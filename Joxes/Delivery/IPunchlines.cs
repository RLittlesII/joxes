using System.Reactive;
using DynamicData;

namespace Joxes.Delivery;

public interface IPunchlines : IObservable<IChangeSet<JokeResponse, CorrelationId>>
{
    IObservable<Unit> Deliver(Categories categories);
}