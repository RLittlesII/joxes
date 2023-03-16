namespace Joxes;

public interface ISignalRReceiver<T>
{
    IObservable<T> Listen();
}