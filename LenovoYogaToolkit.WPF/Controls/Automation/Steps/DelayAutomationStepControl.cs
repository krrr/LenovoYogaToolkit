using LenovoYogaToolkit.Lib.Automation;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class DelayAutomationStepControl : AbstractComboBoxAutomationStepCardControl<Delay>
{
    public DelayAutomationStepControl(IAutomationStep<Delay> step) : base(step)
    {
        Icon = SymbolRegular.Clock24;
        Title = Resource.DelayAutomationStepControl_Title;
        Subtitle = Resource.DelayAutomationStepControl_Message;
    }
}