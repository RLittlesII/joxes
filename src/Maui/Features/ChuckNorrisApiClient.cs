using System.Reactive.Linq;
using DynamicData;

namespace Joxes.Maui.Features;

public class ChuckNorrisApiClient : IChuckNorrisApiClient
{
    public ChuckNorrisApiClient(IChuckNorrisApiContract apiContract) { _apiContract = apiContract; }

    public IObservable<JokeResponse> RandomFromCategory(params Category[] categories) =>
        Observable.FromAsync(_ =>
                                 _apiContract
                                     .RandomFromCategory(categories.OrderBy(x => Randomizer.Random.Next())
                                                                   .ToArray()[0]
                                                                   .Value))
                  // TODO: [rlittlesii: March 11, 2023] I should likely handle the exceptions from the api.
                  .Select(dto => new JokeResponse(new CorrelationId(),
                                                  UserIdentifiers.GetRandom(),
                                                  dto,
                                                  DateTimeOffset.Now))
                  .Cache(_responseCache);

    private readonly IChuckNorrisApiContract _apiContract;
    private readonly SourceCache<JokeResponse, CorrelationId> _responseCache = new(x => x.Id);
}