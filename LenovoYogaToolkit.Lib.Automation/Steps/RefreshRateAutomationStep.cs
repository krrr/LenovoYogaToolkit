using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class RefreshRateAutomationStep : AbstractFeatureAutomationStep<RefreshRate>
{
    [JsonConstructor]
    public RefreshRateAutomationStep(RefreshRate state) : base(state) { }

    public override IAutomationStep DeepCopy() => new RefreshRateAutomationStep(State);
}
