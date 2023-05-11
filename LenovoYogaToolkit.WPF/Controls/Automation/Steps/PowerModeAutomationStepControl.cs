using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class PowerModeAutomationStepControl : AbstractComboBoxAutomationStepCardControl<PowerModeState>
{
    public PowerModeAutomationStepControl(IAutomationStep<PowerModeState> step) : base(step)
    {
        Icon = SymbolRegular.Gauge24;
        Title = Resource.PowerModeAutomationStepControl_Title;
        Subtitle = Resource.PowerModeAutomationStepControl_Message;
    }
}