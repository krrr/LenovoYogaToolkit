﻿using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Resources;
using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class OnStartupAutomationPipelineTrigger : IOnStartupAutomationPipelineTrigger
{
    [JsonIgnore]
    public string DisplayName => Resource.OnStartupAutomationPipelineTrigger_DisplayName;

    public Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        return Task.FromResult(automationEvent is StartupAutomationEvent);
    }

    public Task<bool> IsMatchingState() => Task.FromResult(false);

    public IAutomationPipelineTrigger DeepCopy() => new OnStartupAutomationPipelineTrigger();

    public override bool Equals(object? obj) => obj is OnStartupAutomationPipelineTrigger;

    public override int GetHashCode() => HashCode.Combine(DisplayName);
}