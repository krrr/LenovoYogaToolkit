using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class TouchpadLockAutomationStepControl : AbstractComboBoxAutomationStepCardControl<TouchpadLockState>
{
    public TouchpadLockAutomationStepControl(IAutomationStep<TouchpadLockState> step) : base(step)
    {
        Icon = SymbolRegular.Tablet24;
        Title = Resource.TouchpadLockAutomationStepControl_Title;
        Subtitle = Resource.TouchpadLockAutomationStepControl_Message;
    }
}