﻿using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class OverDriveAutomationStep : AbstractFeatureAutomationStep<OverDriveState>
{
    [JsonConstructor]
    public OverDriveAutomationStep(OverDriveState state) : base(state) { }

    public override IAutomationStep DeepCopy() => new OverDriveAutomationStep(State);
}