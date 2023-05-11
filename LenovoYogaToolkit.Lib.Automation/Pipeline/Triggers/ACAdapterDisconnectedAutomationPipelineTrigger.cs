using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Resources;
using LenovoYogaToolkit.Lib.System;
using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class ACAdapterDisconnectedAutomationPipelineTrigger : IPowerStateAutomationPipelineTrigger
{
    [JsonIgnore]
    public string DisplayName => Resource.ACAdapterDisconnectedAutomationPipelineTrigger_DisplayName;

    public async Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        if (automationEvent is not (PowerStateAutomationEvent or StartupAutomationEvent))
            return false;

        var status = await Power.IsPowerAdapterConnectedAsync().ConfigureAwait(false);
        return status == PowerAdapterStatus.Disconnected;
    }

    public async Task<bool> IsMatchingState()
    {
        var status = await Power.IsPowerAdapterConnectedAsync().ConfigureAwait(false);
        return status == PowerAdapterStatus.Disconnected;
    }

    public IAutomationPipelineTrigger DeepCopy() => new ACAdapterDisconnectedAutomationPipelineTrigger();

    public override bool Equals(object? obj) => obj is ACAdapterDisconnectedAutomationPipelineTrigger;

    public override int GetHashCode() => HashCode.Combine(DisplayName);
}