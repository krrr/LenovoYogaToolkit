using Autofac;
using Autofac.Builder;
using LenovoYogaToolkit.Lib.Listeners;

namespace LenovoYogaToolkit.Lib.Extensions;

public static class RegistrationBuilderExtensions
{
    public static void AutoActivateListener<T>(this IRegistrationBuilder<IListener<T>, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
    {
        registration.OnActivating(e => e.Instance.Start().AsValueTask()).AutoActivate();
    }
}
