using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.System;
using LenovoYogaToolkit.Lib.Utils;
using Microsoft.Win32;

namespace LenovoYogaToolkit.Lib.Listeners;

public class DisplayConfigurationListener : IListener<EventArgs>
{
    private bool _started;

    public event EventHandler<EventArgs>? Changed;

    public Task Start()
    {
        if (_started)
            return Task.CompletedTask;

        SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        _started = true;

        return Task.CompletedTask;
    }

    public Task Stop()
    {
        SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        _started = false;

        return Task.CompletedTask;
    }

    private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
    {
        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Event received.");

        InternalDisplay.SetNeedsRefresh();

        Changed?.Invoke(this, EventArgs.Empty);
    }
}