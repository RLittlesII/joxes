using System.Reactive;

namespace Joxes;

public interface ISignalRBroadcast<T>
{
    IObservable<Unit> Broadcast(T item);
}