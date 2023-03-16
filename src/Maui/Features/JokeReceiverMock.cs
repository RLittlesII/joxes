using System.Reactive.Linq;
using DynamicData;
using Joxes.Delivery;

namespace Joxes.Maui.Features;

public class JokeReceiverMock : IJokeReceiver
{
    public JokeReceiverMock(IPunchlines punchlines)
    {
        _punchlines = punchlines;

        Observable.Interval(TimeSpan.FromSeconds(5))
                  .Select(_ => _punchlines.Deliver(new Categories()))
                  .Subscribe();
    }

    public IObservable<JokeResponse> Listen() =>
        _punchlines.ToCollection()
                   .SelectMany(collection => collection);

    private readonly IPunchlines _punchlines;
}