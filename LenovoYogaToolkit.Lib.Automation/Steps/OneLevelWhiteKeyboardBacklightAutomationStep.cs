using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class OneLevelWhiteKeyboardBacklightAutomationStep : AbstractFeatureAutomationStep<OneLevelWhiteKeyboardBacklightState>
{
    [JsonConstructor]
    public OneLevelWhiteKeyboardBacklightAutomationStep(OneLevelWhiteKeyboardBacklightState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new OneLevelWhiteKeyboardBacklightAutomationStep(State);
}
