#if !DEBUG
using LenovoYogaToolkit.Lib.System;
#endif
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation;
using LenovoYogaToolkit.Lib.Controllers;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.Lib.Features;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.Lib.Utils;
using LenovoYogaToolkit.WPF.Extensions;
using LenovoYogaToolkit.WPF.Resources;
using LenovoYogaToolkit.WPF.Utils;
using LenovoYogaToolkit.WPF.Windows;
using LenovoYogaToolkit.WPF.Windows.Utils;
using WinFormsApp = System.Windows.Forms.Application;
using WinFormsHighDpiMode = System.Windows.Forms.HighDpiMode;

namespace LenovoYogaToolkit.WPF;

public partial class App
{
    private const string MUTEX_NAME = "LenovoYogaToolkit_Mutex_6efcc882-924c-4cbc-8fec-f45c25696f98";
    private const string EVENT_NAME = "LenovoYogaToolkit_Event_6efcc882-924c-4cbc-8fec-f45c25696f98";

    private Mutex? _singleInstanceMutex;
    private EventWaitHandle? _singleInstanceWaitHandle;

    public new static App Current => (App)Application.Current;

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        var flags = new Flags(e.Args);

        Log.Instance.IsTraceEnabled = flags.IsTraceEnabled;

        AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

        EnsureSingleInstance();

        await LocalizationHelper.SetLanguageAsync(true);

        if (!flags.SkipCompatibilityCheck)
        {
            await CheckBasicCompatibilityAsync();
            await CheckCompatibilityAsync();
        }

        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Starting... [version={Assembly.GetEntryAssembly()?.GetName().Version}, build={Assembly.GetEntryAssembly()?.GetBuildDateTime()?.ToString("yyyyMMddHHmmss")}, os={Environment.OSVersion}, dotnet={Environment.Version}]");

        WinFormsApp.SetHighDpiMode(WinFormsHighDpiMode.PerMonitorV2);
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

        IoCContainer.Initialize(
            new Lib.IoCModule(),
            new Lib.Automation.IoCModule(),
            new IoCModule()
        );

        await InitPowerModeFeatureAsync();
        await InitBatteryFeatureAsync();
        await InitAutomationProcessorAsync();

#if !DEBUG
        Autorun.Validate();
#endif

        var mainWindow = new MainWindow
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            TrayTooltipEnabled = !flags.DisableTrayTooltip
        };
        MainWindow = mainWindow;

        IoCContainer.Resolve<ThemeManager>().Apply();

        if (flags.Minimized)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Sending MainWindow to tray...");

            mainWindow.WindowState = WindowState.Minimized;
            mainWindow.Show();
            mainWindow.SendToTray();
        }
        else
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Showing MainWindow...");

            mainWindow.Show();
        }

        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Start up complete");
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _singleInstanceMutex?.Close();
    }

    public void RestartMainWindow()
    {
        if (MainWindow is MainWindow mw)
        {
            mw.SuppressClosingEventHandler = true;
            mw.Close();
        }

        var mainWindow = new MainWindow
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        MainWindow = mainWindow;
        mainWindow.Show();
    }

    public async Task ShutdownAsync()
    {
        try
        {
            if (IoCContainer.TryResolve<PowerModeFeature>() is { } powerModeFeature)
                await powerModeFeature.EnsureAiModeIsOffAsync();
        }
        catch { }

        try
        {
            if (IoCContainer.TryResolve<NativeWindowsMessageListener>() is { } nativeMessageWindowListener)
            {
                await nativeMessageWindowListener.Stop();
            }
        }
        catch { }


        Shutdown();
    }

    private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;

        Log.Instance.ErrorReport("AppDomain_UnhandledException", exception ?? new Exception($"Unknown exception caught: {e.ExceptionObject}"));
        Log.Instance.Trace($"Unhandled exception occurred.", exception);

        MessageBox.Show(string.Format(Resource.UnexpectedException, exception?.Message ?? "Unknown exception.", Constants.BugReportUri),
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        Shutdown(1);
    }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Instance.ErrorReport("Application_DispatcherUnhandledException", e.Exception);
        Log.Instance.Trace($"Unhandled exception occurred.", e.Exception);

        MessageBox.Show(string.Format(Resource.UnexpectedException, e.Exception.Message, Constants.BugReportUri),
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        Shutdown(1);
    }

    private async Task CheckBasicCompatibilityAsync()
    {
        var isCompatible = await Compatibility.CheckBasicCompatibilityAsync();
        if (isCompatible)
            return;

        MessageBox.Show(Resource.IncompatibleDevice_Message, Resource.IncompatibleDevice_Title, MessageBoxButton.OK, MessageBoxImage.Error);

        Shutdown(99);
    }

    private async Task CheckCompatibilityAsync()
    {
        var (isCompatible, mi) = await Compatibility.IsCompatibleAsync();
        if (isCompatible)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Compatibility check passed. [Vendor={mi.Vendor}, Model={mi.Model}, MachineType={mi.MachineType}, BIOS={mi.BiosVersion}]");
            return;
        }

        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Incompatible system detected. [Vendor={mi.Vendor}, Model={mi.Model}, MachineType={mi.MachineType}, BIOS={mi.BiosVersion}]");

        var unsupportedWindow = new UnsupportedWindow(mi);
        unsupportedWindow.Show();

        var result = await unsupportedWindow.ShouldContinue;
        if (result)
        {
            Log.Instance.IsTraceEnabled = true;

            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Compatibility check OVERRIDE. [Vendor={mi.Vendor}, Model={mi.Model}, MachineType={mi.MachineType}, version={Assembly.GetEntryAssembly()?.GetName().Version}, build={Assembly.GetEntryAssembly()?.GetBuildDateTime()?.ToString("yyyyMMddHHmmss") ?? ""}]");
            return;
        }

        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Shutting down... [Vendor={mi.Vendor}, Model={mi.Model}, MachineType={mi.MachineType}]");

        Shutdown(100);
    }

    private void EnsureSingleInstance()
    {
        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Checking for other instances...");

        _singleInstanceMutex = new Mutex(true, MUTEX_NAME, out var isOwned);
        _singleInstanceWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EVENT_NAME);

        if (!isOwned)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Another instance running, closing...");

            _singleInstanceWaitHandle.Set();
            Shutdown();
            return;
        }

        new Thread(() =>
        {
            while (_singleInstanceWaitHandle.WaitOne())
            {
                Current.Dispatcher.BeginInvoke(async () =>
                {
                    if (Current.MainWindow is { } window)
                    {
                        if (Log.Instance.IsTraceEnabled)
                            Log.Instance.Trace($"Another instance started, bringing this one to front instead...");

                        window.BringToForeground();
                    }
                    else
                    {
                        if (Log.Instance.IsTraceEnabled)
                            Log.Instance.Trace($"!!! PANIC !!! This instance is missing main window. Shutting down.");

                        await ShutdownAsync();
                    }
                });
            }
        })
        {
            IsBackground = true
        }.Start();
    }

    private static async Task InitAutomationProcessorAsync()
    {
        try
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Initializing automation processor...");

            var automationProcessor = IoCContainer.Resolve<AutomationProcessor>();
            await automationProcessor.InitializeAsync();
            automationProcessor.RunOnStartup();
        }
        catch (Exception ex)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Couldn't initialize automation processor.", ex);
        }
    }

    private static async Task InitPowerModeFeatureAsync()
    {
        try
        {
            var feature = IoCContainer.Resolve<PowerModeFeature>();
            if (await feature.IsSupportedAsync())
            {
                if (Log.Instance.IsTraceEnabled)
                    Log.Instance.Trace($"Ensuring AI Mode is set...");

                await feature.EnsureAiModeIsSetAsync();
            }
        }
        catch (Exception ex)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Couldn't set AI Mode.", ex);
        }

        try
        {
            var feature = IoCContainer.Resolve<PowerModeFeature>();
            if (await feature.IsSupportedAsync())
            {
                if (Log.Instance.IsTraceEnabled)
                    Log.Instance.Trace($"Ensuring correct power plan is set...");

                await feature.EnsureCorrectPowerPlanIsSetAsync();
            }
        }
        catch (Exception ex)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Couldn't ensure correct power plan.", ex);
        }
    }

    private static async Task InitBatteryFeatureAsync()
    {
        try
        {
            var feature = IoCContainer.Resolve<BatteryFeature>();
            if (await feature.IsSupportedAsync())
            {
                if (Log.Instance.IsTraceEnabled)
                    Log.Instance.Trace($"Ensuring correct battery mode is set...");

                await feature.EnsureCorrectBatteryModeIsSetAsync();
            }
        }
        catch (Exception ex)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Couldn't ensure correct battery mode.", ex);
        }
    }
}