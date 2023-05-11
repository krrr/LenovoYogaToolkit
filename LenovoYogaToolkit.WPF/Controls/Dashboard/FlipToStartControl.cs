using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Dashboard;

public class FlipToStartControl : AbstractToggleFeatureCardControl<FlipToStartState>
{
    protected override FlipToStartState OnState => FlipToStartState.On;

    protected override FlipToStartState OffState => FlipToStartState.Off;

    public FlipToStartControl()
    {
        Icon = SymbolRegular.Power24;
        Title = Resource.FlipToStartControl_Title;
        Subtitle = Resource.FlipToStartControl_Message;
    }
}