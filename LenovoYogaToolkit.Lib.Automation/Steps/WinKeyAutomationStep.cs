using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class WinKeyAutomationStep : AbstractFeatureAutomationStep<WinKeyState>
{
    [JsonConstructor]
    public WinKeyAutomationStep(WinKeyState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new WinKeyAutomationStep(State);
}
