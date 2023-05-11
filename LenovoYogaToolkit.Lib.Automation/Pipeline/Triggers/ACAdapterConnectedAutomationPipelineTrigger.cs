using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Resources;
using LenovoYogaToolkit.Lib.System;
using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class ACAdapterConnectedAutomationPipelineTrigger : IPowerStateAutomationPipelineTrigger
{
    [JsonIgnore]
    public string DisplayName => Resource.ACAdapterConnectedAutomationPipelineTrigger_DisplayName;

    public async Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        if (automationEvent is not (PowerStateAutomationEvent or StartupAutomationEvent))
            return false;

        var status = await Power.IsPowerAdapterConnectedAsync().ConfigureAwait(false);
        return status == PowerAdapterStatus.Connected;
    }

    public async Task<bool> IsMatchingState()
    {
        var status = await Power.IsPowerAdapterConnectedAsync().ConfigureAwait(false);
        return status == PowerAdapterStatus.Connected;
    }

    public IAutomationPipelineTrigger DeepCopy() => new ACAdapterConnectedAutomationPipelineTrigger();

    public override bool Equals(object? obj) => obj is ACAdapterConnectedAutomationPipelineTrigger;

    public override int GetHashCode() => HashCode.Combine(DisplayName);
}