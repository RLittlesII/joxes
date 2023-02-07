using System.Reactive;
using DynamicData;
using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public class ChuckNorrisViewModel : ReactiveObject
{
    public ChuckNorrisViewModel(IChuckNorrisJokes chuckNorrisJokes)
    {
        chuckNorrisJokes
            .Categories()
            .Transform(x => new CategorySelection(x))
            .AutoRefresh(x => x.Excluded)
            .ToCollection()
            .BindTo(this, x => x.Categories);

        Send = ReactiveCommand.CreateFromTask(_ => Task.CompletedTask);
    }

    public ReactiveCommand<Unit, Unit> Send { get; }

    public IEnumerable<CategorySelection> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    private IEnumerable<CategorySelection> _categories;
}