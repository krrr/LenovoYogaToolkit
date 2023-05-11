using System.Threading.Tasks;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Features;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace LenovoYogaToolkit.WPF.Controls.Dashboard;

public class TouchpadLockControl : AbstractToggleFeatureCardControl<TouchpadLockState>
{
    private readonly DriverKeyListener _listener = IoCContainer.Resolve<DriverKeyListener>();

    protected override TouchpadLockState OnState => TouchpadLockState.On;

    protected override TouchpadLockState OffState => TouchpadLockState.Off;

    public TouchpadLockControl()
    {
        Icon = SymbolRegular.Tablet24;
        Title = Resource.TouchpadLockControl_Title;
        Subtitle = Resource.TouchpadLockControl_Message;

        _listener.Changed += Listener_Changed;
    }

    protected override async Task OnStateChange(ToggleSwitch toggle, IFeature<TouchpadLockState> feature)
    {
        await _listener.Stop();
        await base.OnStateChange(toggle, feature);
        await _listener.Start();
    }

    private void Listener_Changed(object? sender, DriverKey e) => Dispatcher.Invoke(async () =>
    {
        if (!IsLoaded || !IsVisible)
            return;

        if (e.HasFlag(DriverKey.Fn_F10))
            await RefreshAsync();
    });
}