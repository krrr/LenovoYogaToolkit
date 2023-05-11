using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class BatteryAutomationStep : AbstractFeatureAutomationStep<BatteryState>
{
    [JsonConstructor]
    public BatteryAutomationStep(BatteryState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new BatteryAutomationStep(State);
}
