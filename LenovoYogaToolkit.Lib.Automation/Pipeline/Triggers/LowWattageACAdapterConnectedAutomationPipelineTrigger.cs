using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Resources;
using LenovoYogaToolkit.Lib.System;
using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class LowWattageACAdapterConnectedAutomationPipelineTrigger : IPowerStateAutomationPipelineTrigger
{
    [JsonIgnore]
    public string DisplayName => Resource.LowWattageACAdapterConnectedAutomationPipelineTrigger_DisplayName;

    public async Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        if (automationEvent is not (PowerStateAutomationEvent or StartupAutomationEvent))
            return false;

        var result = await Power.IsPowerAdapterConnectedAsync().ConfigureAwait(false);
        return result == PowerAdapterStatus.ConnectedLowWattage;
    }

    public async Task<bool> IsMatchingState()
    {
        var result = await Power.IsPowerAdapterConnectedAsync().ConfigureAwait(false);
        return result == PowerAdapterStatus.ConnectedLowWattage;
    }

    public IAutomationPipelineTrigger DeepCopy() => new LowWattageACAdapterConnectedAutomationPipelineTrigger();

    public override bool Equals(object? obj) => obj is LowWattageACAdapterConnectedAutomationPipelineTrigger;

    public override int GetHashCode() => HashCode.Combine(DisplayName);
}