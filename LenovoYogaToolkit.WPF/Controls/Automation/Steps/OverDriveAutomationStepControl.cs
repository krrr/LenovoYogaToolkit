using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class OverDriveAutomationStepControl : AbstractComboBoxAutomationStepCardControl<OverDriveState>
{
    public OverDriveAutomationStepControl(IAutomationStep<OverDriveState> step) : base(step)
    {
        Icon = SymbolRegular.TopSpeed24;
        Title = Resource.OverDriveAutomationStepControl_Title;
        Subtitle = Resource.OverDriveAutomationStepControl_Message;
    }
}