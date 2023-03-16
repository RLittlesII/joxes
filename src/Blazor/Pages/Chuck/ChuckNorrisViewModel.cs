using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Joxes.Delivery;
using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public class ChuckNorrisViewModel : ReactiveObject
{
    public ChuckNorrisViewModel(IChuckNorrisJokes chuckNorrisJokes, IPunchlines punchlines, UserId userId)
    {
        UserId = userId;
        Send = ReactiveCommand.CreateFromObservable(() => punchlines.Deliver(
                                                        new Categories(Categories.Where(x => !x.Excluded)
                                                                                 .Select(x => x.Category))));

        punchlines
            .Transform(response => new DeliveredJokeViewModel(response))
            .ToCollection()
            .Do(_ => { })
            .BindTo(this, x => x.Jokes);

        chuckNorrisJokes
            .Categories()
            .Filter(category => !Excluded.Contains(category.Value))
            .Transform(x => new CategorySelection(x))
            .AutoRefresh(categorySelection => categorySelection.Excluded)
            .ToCollection()
            .Do(_ => { })
            .BindTo(this, x => x.Categories);
    }

    public UserId UserId { get; }
    public ReactiveCommand<Unit, Unit> Send { get; }

    public IEnumerable<DeliveredJokeViewModel> Jokes
    {
        get => _jokes;
        set => this.RaiseAndSetIfChanged(ref _jokes, value);
    }

    public IEnumerable<CategorySelection> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    private IEnumerable<CategorySelection> _categories = Enumerable.Empty<CategorySelection>();
    private IEnumerable<DeliveredJokeViewModel> _jokes = Enumerable.Empty<DeliveredJokeViewModel>();

    private static readonly IReadOnlyList<string> Excluded =
        Enum.GetValues<IgnoreType>()
            .Select(ignoreType => ignoreType.ToString()
                                            .ToLowerInvariant())
            .ToList()
            .AsReadOnly();
}