using System;
using System.Threading.Tasks;
using System.Timers;
using LenovoYogaToolkit.Lib.Listeners;

namespace LenovoYogaToolkit.Lib.Automation.Listeners;

public class TimeAutomationListener : IListener<Time>
{
    public event EventHandler<Time>? Changed;

    private readonly Timer _timer;

    public TimeAutomationListener()
    {
        _timer = new Timer(60_000);
        _timer.Elapsed += Timer_Elapsed;
        _timer.AutoReset = true;
    }

    public Task Start()
    {
        if (!_timer.Enabled)
            _timer.Enabled = true;

        return Task.CompletedTask;
    }

    public Task Stop()
    {
        _timer.Enabled = false;

        return Task.CompletedTask;
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        var now = DateTime.UtcNow;
        Changed?.Invoke(this, new() { Hour = now.Hour, Minute = now.Minute });
    }
}