using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Dashboard;

public class WhiteKeyboardBacklightControl : AbstractComboBoxFeatureCardControl<WhiteKeyboardBacklightState>
{
    private readonly DriverKeyListener _listener = IoCContainer.Resolve<DriverKeyListener>();

    public WhiteKeyboardBacklightControl()
    {
        Icon = SymbolRegular.Keyboard24;
        Title = Resource.WhiteKeyboardBacklightControl_Title;
        Subtitle = Resource.WhiteKeyboardBacklightControl_Message;

        _listener.Changed += ListenerChanged;
    }

    private void ListenerChanged(object? sender, DriverKey e) => Dispatcher.Invoke(async () =>
    {
        if (!IsLoaded || !IsVisible)
            return;

        if (e.HasFlag(DriverKey.Fn_Space))
            await RefreshAsync();
    });
}