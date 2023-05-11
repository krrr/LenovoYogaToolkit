using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class AlwaysOnUsbAutomationStep : AbstractFeatureAutomationStep<AlwaysOnUSBState>
{
    [JsonConstructor]
    public AlwaysOnUsbAutomationStep(AlwaysOnUSBState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new AlwaysOnUsbAutomationStep(State);
}
