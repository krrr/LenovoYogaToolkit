using LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;

namespace LenovoYogaToolkit.WPF.Windows.Automation.TabItemContent;

public interface IAutomationPipelineTriggerTabItemContent<out T> where T : IAutomationPipelineTrigger
{
    T GetTrigger();
}
