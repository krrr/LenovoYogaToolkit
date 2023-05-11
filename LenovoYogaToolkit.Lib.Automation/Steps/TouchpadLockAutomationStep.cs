using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class TouchpadLockAutomationStep : AbstractFeatureAutomationStep<TouchpadLockState>
{
    [JsonConstructor]
    public TouchpadLockAutomationStep(TouchpadLockState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new TouchpadLockAutomationStep(State);
}
