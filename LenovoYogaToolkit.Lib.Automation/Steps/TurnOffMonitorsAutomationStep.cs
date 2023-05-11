using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Listeners;

namespace LenovoYogaToolkit.Lib.Automation.Steps;

public class TurnOffMonitorsAutomationStep : IAutomationStep
{
    private readonly NativeWindowsMessageListener _nativeWindowsMessageListener = IoCContainer.Resolve<NativeWindowsMessageListener>();

    public Task<bool> IsSupportedAsync() => Task.FromResult(true);

    public Task RunAsync() => _nativeWindowsMessageListener.TurnOffMonitorAsync();

    public IAutomationStep DeepCopy() => new TurnOffMonitorsAutomationStep();
}
