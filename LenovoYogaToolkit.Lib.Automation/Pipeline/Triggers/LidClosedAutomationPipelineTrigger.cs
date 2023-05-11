using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Resources;
using LenovoYogaToolkit.Lib.Listeners;
using Newtonsoft.Json;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class LidClosedAutomationPipelineTrigger : INativeWindowsMessagePipelineTrigger
{
    [JsonIgnore]
    public string DisplayName => Resource.LidClosedAutomationPipelineTrigger_DisplayName;

    public Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        var result = automationEvent is NativeWindowsMessageEvent { Message: NativeWindowsMessage.LidClosed };
        return Task.FromResult(result);
    }

    public Task<bool> IsMatchingState()
    {
        var listener = IoCContainer.Resolve<NativeWindowsMessageListener>();
        var result = !listener.IsLidOpen;
        return Task.FromResult(result);
    }

    public IAutomationPipelineTrigger DeepCopy() => new LidClosedAutomationPipelineTrigger();

    public override bool Equals(object? obj) => obj is LidClosedAutomationPipelineTrigger;

    public override int GetHashCode() => HashCode.Combine(DisplayName);
}
