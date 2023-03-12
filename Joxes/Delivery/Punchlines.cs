using System.Reactive;
using System.Reactive.Linq;

namespace Joxes.Delivery;

public class Punchlines : IPunchlines
{
    public Punchlines(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, UserId userId)
    {
        _jsonSerializer = jsonSerializer;
        _userId = userId;
        _httpClient = httpClientFactory
            .CreateClient("Functions");
    }

    /// <inheritdoc />
    public IObservable<Unit> Deliver(Categories categories)
    {
        var serialize = _jsonSerializer.Serialize(new JokeRequest(_userId, categories));
        return Observable.FromAsync(token => _httpClient
                                             .PostAsync("api/DeliveryFunction",
                                                        new StringContent(serialize),
                                                        token))
                         // TODO: [rlittlesii: March 11, 2023] I should likely handle the exceptions from the api.
                         .Select(_ => Unit.Default);
    }

    private readonly IJsonSerializer _jsonSerializer;
    private readonly UserId _userId;
    private readonly HttpClient _httpClient;
}

public interface IPunchlines
{
    IObservable<Unit> Deliver(Categories categories);
}