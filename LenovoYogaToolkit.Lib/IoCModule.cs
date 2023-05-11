using Autofac;
using LenovoYogaToolkit.Lib.Controllers;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.Lib.Features;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.Lib.PackageDownloader;
using LenovoYogaToolkit.Lib.Settings;
using LenovoYogaToolkit.Lib.System;
using LenovoYogaToolkit.Lib.Utils;

namespace LenovoYogaToolkit.Lib;

public class IoCModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<FnKeys>();
        builder.Register<LegionZone>();
        builder.Register<Vantage>();

        builder.Register<ApplicationSettings>();
        builder.Register<BalanceModeSettings>();
        builder.Register<PackageDownloaderSettings>();
        builder.Register<SunriseSunsetSettings>();

        builder.Register<AlwaysOnUSBFeature>();
        builder.Register<BatteryFeature>();
        builder.Register<DpiScaleFeature>();
        builder.Register<FlipToStartFeature>();
        builder.Register<FnLockFeature>();
        builder.Register<GSyncFeature>();
        builder.Register<HDRFeature>();
        builder.Register<HybridModeFeature>();
        builder.Register<IGPUModeFeature>();
        builder.Register<MicrophoneFeature>();
        builder.Register<OneLevelWhiteKeyboardBacklightFeature>();
        builder.Register<PowerModeFeature>();
        builder.Register<RefreshRateFeature>();
        builder.Register<ResolutionFeature>();
        builder.Register<TouchpadLockFeature>();
        builder.Register<WhiteKeyboardBacklightFeature>();
        builder.Register<WinKeyFeature>();

        builder.Register<DisplayBrightnessListener>().AutoActivateListener();
        builder.Register<DisplayConfigurationListener>().AutoActivateListener();
        builder.Register<DriverKeyListener>().AutoActivateListener();
        builder.Register<LightingChangeListener>().AutoActivateListener();
        builder.Register<NativeWindowsMessageListener>().AutoActivateListener();
        builder.Register<PowerModeListener>().AutoActivateListener();
        builder.Register<PowerPlanListener>().AutoActivateListener();
        builder.Register<PowerStateListener>().AutoActivateListener();
        builder.Register<SpecialKeyListener>().AutoActivateListener();
        builder.Register<SystemThemeListener>().AutoActivateListener();
        builder.Register<ThermalModeListener>().AutoActivateListener();
        builder.Register<WinKeyListener>().AutoActivateListener();

        builder.Register<AIModeController>();
        builder.Register<DisplayBrightnessController>();
        builder.Register<GPUController>();
        builder.Register<PowerPlanController>();

        builder.Register<UpdateChecker>();
        builder.Register<WarrantyChecker>();

        builder.Register<PackageDownloaderFactory>();
        builder.Register<PCSupportPackageDownloader>();
        builder.Register<VantagePackageDownloader>();

        builder.Register<SunriseSunset>();
    }
}
