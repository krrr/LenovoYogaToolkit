using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class WhiteKeyboardBacklightAutomationStepControl : AbstractComboBoxAutomationStepCardControl<WhiteKeyboardBacklightState>
{
    public WhiteKeyboardBacklightAutomationStepControl(IAutomationStep<WhiteKeyboardBacklightState> step) : base(step)
    {
        Icon = SymbolRegular.Keyboard24;
        Title = Resource.WhiteKeyboardBacklightAutomationStepControl_Title;
        Subtitle = Resource.WhiteKeyboardBacklightAutomationStepControl_Message;
    }
}