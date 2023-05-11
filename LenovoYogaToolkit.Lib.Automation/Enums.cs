using System.ComponentModel.DataAnnotations;
using LenovoYogaToolkit.Lib.Automation.Resources;

namespace LenovoYogaToolkit.Lib.Automation;

public enum DeactivateGPUAutomationStepState
{
    [Display(ResourceType = typeof(Resource), Name = "DeactivateGPUAutomationStepState_KillApps")]
    KillApps,
    [Display(ResourceType = typeof(Resource), Name = "DeactivateGPUAutomationStepState_RestartGPU")]
    RestartGPU,
}