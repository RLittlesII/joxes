using System.Reactive.Linq;
using Rocket.Surgery.Airframe.Data;

namespace Joxes;

public class TightFive : IObservable<Joke>
{
    public TightFive(IChuckNorrisJokeApiClient client) { }
    public IDisposable Subscribe(IObserver<Joke> observer) { return Observable.Empty<Joke>().Subscribe(observer); }
}