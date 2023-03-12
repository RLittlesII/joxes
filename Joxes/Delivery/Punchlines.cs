using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Joxes.Serialization;

namespace Joxes.Delivery;

public class Punchlines : IPunchlines
{
    public Punchlines(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, UserId userId)
    {
        _jsonSerializer = jsonSerializer;
        _userId = userId;
        _httpClient = httpClientFactory
            .CreateClient("Functions");

        _responseCache = new SourceCache<JokeResponse, CorrelationId>(x => x.Id);
    }

    /// <inheritdoc />
    public IObservable<Unit> Deliver(Categories categories)
    {
        var serialize = _jsonSerializer.Serialize(new JokeRequest(_userId, categories));
        return Observable.FromAsync(token => _httpClient
                                             .PostAsync("api/DeliveryFunction",
                                                        new StringContent(serialize),
                                                        token))
                         // HACK: [rlittlesii: March 11, 2023] This is ugly
                         .Select(_ =>
                         {
                             if (!_.IsSuccessStatusCode)
                             {
                                 Observable.Empty<JokeResponse>();
                             }
                             return _jsonSerializer.Deserialize<JokeResponse>(_.Content.ReadAsStringAsync()
                                                                               .GetAwaiter()
                                                                               .GetResult());
                         })
                         .Do(response => _responseCache.AddOrUpdate(response))
                         // TODO: [rlittlesii: March 11, 2023] I should likely handle the exceptions from the api.
                         .Select(_ => Unit.Default);
    }

    private readonly IJsonSerializer _jsonSerializer;
    private readonly UserId _userId;
    private readonly HttpClient _httpClient;
    private readonly SourceCache<JokeResponse, CorrelationId> _responseCache;
}

public interface IPunchlines
{
    IObservable<Unit> Deliver(Categories categories);
}