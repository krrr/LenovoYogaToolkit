using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class FnLockAutomationStepControl : AbstractComboBoxAutomationStepCardControl<FnLockState>
{
    public FnLockAutomationStepControl(IAutomationStep<FnLockState> step) : base(step)
    {
        Icon = SymbolRegular.Keyboard24;
        Title = Resource.FnLockAutomationStepControl_Title;
        Subtitle = Resource.FnLockAutomationStepControl_Message;
    }
}