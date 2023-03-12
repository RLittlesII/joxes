using System.Reactive;
using DynamicData;
using Joxes.Delivery;
using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public class ChuckNorrisViewModel : ReactiveObject
{
    public ChuckNorrisViewModel(IChuckNorrisJokes chuckNorrisJokes, IPunchlines punchlines)
    {
        Send = ReactiveCommand.CreateFromObservable(() => punchlines.Deliver(
                                                        new Categories(Categories.Where(x => !x.Excluded)
                                                                           .Select(x => x.Category))));

        Send
            .Subscribe(_ => { });

        chuckNorrisJokes
            .Categories()
            .Filter(category => !Excluded.Contains(category.Value))
            .Transform(x => new CategorySelection(x))
            .AutoRefresh(categorySelection => categorySelection.Excluded)
            .ToCollection()
            .BindTo(this, x => x.Categories);
    }

    public UserId UserId { get; }

    public ReactiveCommand<Unit, Unit> Send { get; }

    public IEnumerable<CategorySelection> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    private IEnumerable<CategorySelection> _categories;

    private static readonly IReadOnlyList<string> Excluded = Enum.GetValues<IgnoreType>()
                                                                 .Select(x => x.ToString()
                                                                               .ToLowerInvariant())
                                                                 .ToList()
                                                                 .AsReadOnly();
}