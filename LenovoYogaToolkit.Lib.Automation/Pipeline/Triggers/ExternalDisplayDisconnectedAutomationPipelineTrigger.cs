using System;
using System.Linq;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Resources;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.Lib.System;
using Newtonsoft.Json;
using WindowsDisplayAPI;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class ExternalDisplayDisconnectedAutomationPipelineTrigger : INativeWindowsMessagePipelineTrigger
{
    [JsonIgnore] public string DisplayName => Resource.ExternalDisplayDisconnectedAutomationPipelineTrigger_DisplayName;

    public Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        var result = automationEvent is NativeWindowsMessageEvent { Message: NativeWindowsMessage.MonitorDisconnected };
        return Task.FromResult(result);
    }

    public Task<bool> IsMatchingState()
    {
        var displays = Display.GetDisplays();
        var internalDisplay = InternalDisplay.Get();
        if (internalDisplay is not null)
            displays = displays.Where(d => d.DevicePath != internalDisplay.DevicePath);
        var result = displays.IsEmpty();
        return Task.FromResult(result);
    }

    public IAutomationPipelineTrigger DeepCopy() => new ExternalDisplayDisconnectedAutomationPipelineTrigger();

    public override bool Equals(object? obj) => obj is ExternalDisplayDisconnectedAutomationPipelineTrigger;

    public override int GetHashCode() => HashCode.Combine(DisplayName);
}
