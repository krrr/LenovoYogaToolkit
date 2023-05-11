using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class BatteryAutomationStepControl : AbstractComboBoxAutomationStepCardControl<BatteryState>
{
    public BatteryAutomationStepControl(IAutomationStep<BatteryState> step) : base(step)
    {
        Icon = SymbolRegular.BatteryCharge24;
        Title = Resource.BatteryAutomationStepControl_Title;
        Subtitle = Resource.BatteryAutomationStepControl_Message;
    }
}