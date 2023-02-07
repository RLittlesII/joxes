using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Joxes;
using ReactiveUI;
using Rocket.Surgery.Airframe.Data;

namespace Blazor.Pages;

public class ChuckNorrisViewModel : ReactiveObject
{
    public ChuckNorrisViewModel(IChuckNorrisJokes chuckNorrisJokes)
    {
        chuckNorrisJokes
            .Categories()
            .Bind(out var categories)
            .Subscribe();

        Categories = categories;
    }

    public Category Category
    {
        get => _category;
        set => this.RaiseAndSetIfChanged(ref _category, value);
    }

    public ReadOnlyObservableCollection<Category> Categories { get; }

    private Category _category;
}

public interface IChuckNorrisJokes : IChuckNorrisJokeService
{
    IObservable<IChangeSet<Category, string>> Categories();
}

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