using DynamicData;

namespace Joxes;

public class TightFiveDelivery : IObservable<IChangeSet<Joke, Guid>>
{
    public TightFiveDelivery(TightFive tightFive)
    {
        var cache = new SourceCache<Joke, Guid>(x => x.Id);

        _delivery = cache.Connect()
                         .RefCount();

        tightFive.Subscribe(_ => cache.AddOrUpdate(_));
    }

    public IDisposable Subscribe(IObserver<IChangeSet<Joke, Guid>> observer) => _delivery.Subscribe(observer);

    private readonly IObservable<IChangeSet<Joke, Guid>> _delivery;
}