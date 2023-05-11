using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.System;
using LenovoYogaToolkit.Lib.Utils;

namespace LenovoYogaToolkit.Lib.Listeners;

public class SystemThemeListener : IListener<EventArgs>
{
    public event EventHandler<EventArgs>? Changed;

    private IDisposable? _darkModeListener;
    private IDisposable? _colorizationColorListener;

    private RGBColor? _currentRegColor;

    private bool _started;

    public Task Start()
    {
        if (_started)
            return Task.CompletedTask;

        _darkModeListener = SystemTheme.GetDarkModeListener(OnDarkModeChanged);
        _colorizationColorListener = SystemTheme.GetColorizationColorListener(OnColorizationColorChanged);

        _started = true;

        return Task.CompletedTask;
    }

    private void OnDarkModeChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void OnColorizationColorChanged()
    {
        try
        {
            var color = SystemTheme.GetColorizationColor();

            // Ignore alpha channel transition events
            if (color.Equals(_currentRegColor))
                return;

            _currentRegColor = color;

            Changed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            if (Log.Instance.IsTraceEnabled)
                Log.Instance.Trace($"Failed to notify on accent color change.", ex);
        }
    }

    public Task Stop()
    {
        _darkModeListener?.Dispose();
        _colorizationColorListener?.Dispose();

        _started = false;

        return Task.CompletedTask;
    }
}