using DynamicData;

namespace Joxes.Maui.Features;

public interface IChuckNorrisAggregator
{
    IObservable<IChangeSet<Joke, CorrelationId>> Jokes();
}