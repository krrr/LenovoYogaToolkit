using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Dashboard;

public class OverDriveControl : AbstractToggleFeatureCardControl<OverDriveState>
{
    protected override OverDriveState OnState => OverDriveState.On;

    protected override OverDriveState OffState => OverDriveState.Off;

    public OverDriveControl()
    {
        Icon = SymbolRegular.TopSpeed24;
        Title = Resource.OverDriveControl_Title;
        Subtitle = Resource.OverDriveControl_Message;
    }
}