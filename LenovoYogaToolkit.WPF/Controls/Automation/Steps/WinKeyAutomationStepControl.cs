using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class WinKeyAutomationStepControl : AbstractComboBoxAutomationStepCardControl<WinKeyState>
{
    public WinKeyAutomationStepControl(IAutomationStep<WinKeyState> step) : base(step)
    {
        Icon = SymbolRegular.Keyboard24;
        Title = Resource.WinKeyAutomationStepControl_Title;
        Subtitle = Resource.WinKeyAutomationStepControl_Message;
    }
}