using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Joxes.Serialization;

namespace Joxes.Delivery;

public class Punchlines : IPunchlines
{
    public Punchlines(IChuckNorrisApiContract chuckNorrisApiContract,
                      IJsonSerializer jsonSerializer,
                      IJokeBroadcast jokeBroadcast,
                      UserId userId)
    {
        _jsonSerializer = jsonSerializer;
        _jokeBroadcast = jokeBroadcast;
        _userId = userId;
        _chuckNorrisApiContract = chuckNorrisApiContract;
        _responseCache = new SourceCache<JokeResponse, CorrelationId>(x => x.Id);
        _responses = _responseCache
                     .Connect()
                     .RefCount();
    }

    /// <inheritdoc />
    public IObservable<Unit> Deliver(Categories categories)
    {
        var jokeRequest = new JokeRequest(_userId, categories);
        return Observable.FromAsync(_ =>
                                        _chuckNorrisApiContract
                                            .RandomFromCategory(jokeRequest.Categories.OrderBy(x => Random.Next())
                                                                           .ToArray()[0]
                                                                           .Value))
                         // TODO: [rlittlesii: March 11, 2023] I should likely handle the exceptions from the api.
                         .Select(dto => new JokeResponse(jokeRequest.Id, jokeRequest.UserId, dto, DateTimeOffset.Now))
                         .Cache(_responseCache)
                         .SelectMany(jokeResponse => _jokeBroadcast.Broadcast(jokeResponse));
    }

    public IDisposable Subscribe(IObserver<IChangeSet<JokeResponse, CorrelationId>> observer) =>
        _responses
            .Subscribe(observer);

    private readonly IChuckNorrisApiContract _chuckNorrisApiContract;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IJokeBroadcast _jokeBroadcast;
    private readonly UserId _userId;
    private readonly SourceCache<JokeResponse, CorrelationId> _responseCache;
    private readonly IObservable<IChangeSet<JokeResponse, CorrelationId>> _responses;
    private static readonly Random Random = new();
}