using System.Reactive;
using DynamicData;
using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public class ChuckNorrisViewModel : ReactiveObject
{
    public ChuckNorrisViewModel(IChuckNorrisJokes chuckNorrisJokes, IHttpClientFactory httpClientFactory)
    {
        chuckNorrisJokes
            .Categories()
            .Transform(x => new CategorySelection(x))
            .AutoRefresh(x => x.Excluded)
            .ToCollection()
            .BindTo(this, x => x.Categories);

        Send = ReactiveCommand.CreateFromTask<Unit, HttpResponseMessage>(_ =>
                                                                             httpClientFactory
                                                                                 .CreateClient("Functions")
                                                                                 .PostAsync("api/DeliveryFunction",
                                                                                     null,
                                                                                     CancellationToken.None));

        Send
            .Subscribe(_ => { });
    }

    public ReactiveCommand<Unit, HttpResponseMessage> Send { get; }

    public IEnumerable<CategorySelection> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    private IEnumerable<CategorySelection> _categories;
}