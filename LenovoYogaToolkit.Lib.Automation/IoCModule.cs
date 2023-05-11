using Autofac;
using LenovoYogaToolkit.Lib.Automation.Listeners;
using LenovoYogaToolkit.Lib.Automation.Utils;
using LenovoYogaToolkit.Lib.Extensions;

namespace LenovoYogaToolkit.Lib.Automation;

public class IoCModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<AutomationSettings>();

        builder.Register<GameAutomationListener>(true);
        builder.Register<ProcessAutomationListener>(true);
        builder.Register<TimeAutomationListener>(true);

        builder.Register<AutomationProcessor>();
    }
}