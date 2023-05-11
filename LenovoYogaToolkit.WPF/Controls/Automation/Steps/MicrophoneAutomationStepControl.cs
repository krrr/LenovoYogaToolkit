using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class MicrophoneAutomationStepControl : AbstractComboBoxAutomationStepCardControl<MicrophoneState>
{
    public MicrophoneAutomationStepControl(IAutomationStep<MicrophoneState> step) : base(step)
    {
        Icon = SymbolRegular.Mic24;
        Title = Resource.MicrophoneAutomationStepControl_Title;
        Subtitle = Resource.MicrophoneAutomationStepControl_Message;
    }
}