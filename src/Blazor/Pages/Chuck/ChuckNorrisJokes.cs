using System.Reactive.Linq;
using DynamicData;
using Rocket.Surgery.Airframe.Data;

namespace Joxes.Blazor.Pages.Chuck;

public class ChuckNorrisJokes : IChuckNorrisJokes
{
    public ChuckNorrisJokes(IChuckNorrisJokeApiClient apiClient)
    {
        _apiClient = apiClient;

        _jokes = new SourceCache<Joke, Guid>(joke => joke.Id);

        _categories = new SourceCache<Category, string>(category => category.Value);

        _jokes
            .Connect()
            .RefCount()
            .Transform(joke => joke.Categories)
            .ToCollection()
            .SelectMany(collection => collection.SelectMany(list => list))
            .ToObservableChangeSet(category => category.Value)
            .PopulateInto(_categories);
    }

    IObservable<ChuckNorrisJoke> IChuckNorrisJokeService.Random() => _apiClient.Random();

    IObservable<ChuckNorrisJoke> IChuckNorrisJokeService.Random(params string[] categories) =>
        categories.ToObservable()
                  .SelectMany(category =>
                                  _apiClient
                                      .RandomFromCategory(category)
                                      .SelectMany(jokes => jokes));

    IObservable<IChangeSet<ChuckNorrisJoke, string>> IChuckNorrisJokeService.Query(string query) =>
        _apiClient
            .Search(query)
            .ToObservableChangeSet(x => x.Id);

    IObservable<IChangeSet<ChuckNorrisJoke, string>> IChuckNorrisJokeService.Query(string query, bool clearCache) =>
        ((IChuckNorrisJokeService) this).Query(query);

    public IObservable<IChangeSet<Category, string>> Categories() =>
        _apiClient
            .Categories()
            .Select(categories =>
                        categories.Select(category => new Category(category)))
            .Cache(_categories, true);

    private readonly IChuckNorrisJokeApiClient _apiClient;
    private readonly SourceCache<Joke, Guid> _jokes;
    private SourceCache<Category, string> _categories;
}