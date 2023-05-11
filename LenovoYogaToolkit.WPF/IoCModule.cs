using Autofac;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.WPF.Settings;
using LenovoYogaToolkit.WPF.Utils;

namespace LenovoYogaToolkit.WPF;

public class IoCModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {

        builder.Register<ThemeManager>().AutoActivate();
        builder.Register<NotificationsManager>().AutoActivate();

        builder.Register<DashboardSettings>();
    }
}