using System.Reactive.Linq;
using DynamicData;

namespace Joxes.Maui.Features;

public class ChuckNorrisAggregator : IChuckNorrisAggregator
{
    public ChuckNorrisAggregator(IChuckNorrisApiClient apiClient)
    {
        _apiClient = apiClient;
        _jokeChanges = _jokes.Connect()
                             .RefCount();

        Observable.Interval(TimeSpan.FromSeconds(5))
                  .Select(x => Jokes())
                  .Switch()
                  .Subscribe();

    }

    public IObservable<IChangeSet<Joke, CorrelationId>> Jokes() =>
        _apiClient.RandomFromCategory(new Categories().ToArray())
                  .Select(jokeResponse => new Joke(jokeResponse.Id,
                                                   jokeResponse.UserId,
                                                   new JokeId(jokeResponse.JokeDto.Id),
                                                   jokeResponse.JokeDto.IconUrl,
                                                   new Punchline(jokeResponse.JokeDto.Value),
                                                   jokeResponse.JokeDto.Categories.Select(cat => new Category(cat)),
                                                   jokeResponse.Timestamp))
                  .Cache(_jokes)
                  .Select(_ => _jokeChanges)
                  .Switch();

    private readonly IChuckNorrisApiClient _apiClient;
    private SourceCache<Joke, CorrelationId> _jokes = new SourceCache<Joke, CorrelationId>(x => x.Id);
    private readonly IObservable<IChangeSet<Joke, CorrelationId>> _jokeChanges;
}