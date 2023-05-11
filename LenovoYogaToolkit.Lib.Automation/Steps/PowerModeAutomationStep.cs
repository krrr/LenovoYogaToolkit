using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class PowerModeAutomationStep : AbstractFeatureAutomationStep<PowerModeState>
{
    [JsonConstructor]
    public PowerModeAutomationStep(PowerModeState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new PowerModeAutomationStep(State);
}
