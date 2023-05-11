using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Automation.Listeners;
using LenovoYogaToolkit.Lib.Automation.Resources;

namespace LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

public class GamesAreRunningAutomationPipelineTrigger : IGameAutomationPipelineTrigger
{
    public string DisplayName => Resource.GamesAreRunningAutomationPipelineTrigger_DisplayName;

    public Task<bool> IsMatchingEvent(IAutomationEvent automationEvent)
    {
        var result = automationEvent is GameAutomationEvent { Started: true };
        return Task.FromResult(result);
    }

    public Task<bool> IsMatchingState()
    {
        var listener = IoCContainer.Resolve<GameAutomationListener>();
        var result = listener.AreGamesRunning();
        return Task.FromResult(result);
    }

    public IAutomationPipelineTrigger DeepCopy() => new GamesAreRunningAutomationPipelineTrigger();
}
