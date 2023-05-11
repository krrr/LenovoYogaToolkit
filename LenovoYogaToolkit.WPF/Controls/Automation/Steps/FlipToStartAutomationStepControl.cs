using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class FlipToStartAutomationStepControl : AbstractComboBoxAutomationStepCardControl<FlipToStartState>
{
    public FlipToStartAutomationStepControl(IAutomationStep<FlipToStartState> step) : base(step)
    {
        Icon = SymbolRegular.Power24;
        Title = Resource.FlipToStartAutomationStepControl_Title;
        Subtitle = Resource.FlipToStartAutomationStepControl_Message;
    }
}