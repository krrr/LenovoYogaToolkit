using System;
using System.Threading.Tasks;

namespace LenovoYogaToolkit.Lib.Listeners;

public interface IListener<T>
{
    event EventHandler<T>? Changed;

    Task Start();

    Task Stop();
}

public interface INotifyingListener<T> : IListener<T>
{
    Task NotifyAsync(T value);
}