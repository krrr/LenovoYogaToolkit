using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class OneLevelWhiteKeyboardBacklightAutomationStepControl : AbstractComboBoxAutomationStepCardControl<OneLevelWhiteKeyboardBacklightState>
{
    public OneLevelWhiteKeyboardBacklightAutomationStepControl(IAutomationStep<OneLevelWhiteKeyboardBacklightState> step) : base(step)
    {
        Icon = SymbolRegular.Keyboard24;
        Title = Resource.OneLevelWhiteKeyboardBacklightAutomationStepControl_Title;
        Subtitle = Resource.OneLevelWhiteKeyboardBacklightAutomationStepControl_Message;
    }
}