using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public class CategorySelection : ReactiveObject
{
    public CategorySelection(Category category)
    {
        Category = category.Value;
    }

    public string Category { get; }

    public bool Excluded
    {
        get => _excluded;
        set => this.RaiseAndSetIfChanged(ref _excluded, value);
    }

    private bool _excluded;
}