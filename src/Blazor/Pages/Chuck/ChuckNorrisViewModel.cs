using System.Reactive;
using System.Text.Json;
using System.Text.Json.Serialization;
using DynamicData;
using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public class ChuckNorrisViewModel : ReactiveObject
{
    public ChuckNorrisViewModel(IChuckNorrisJokes chuckNorrisJokes, IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, UserId userId)
    {
        UserId = userId;

        Send = ReactiveCommand.CreateFromTask<Unit, HttpResponseMessage>(_ =>
        {
            var jokeRequest = new JokeRequest(UserId,
                                              Categories.Where(x => !x.Excluded)
                                                        .Select(x => x.Category));
            var serialize = jsonSerializer.Serialize(jokeRequest);
            return httpClientFactory
                   .CreateClient("Functions")
                   .PostAsync("api/DeliveryFunction",
                              new StringContent(serialize),
                              CancellationToken.None);
        });

        Send
            .Subscribe(_ => { });

        chuckNorrisJokes
            .Categories()
            .Transform(x => new CategorySelection(x))
            .AutoRefresh(x => x.Excluded)
            .ToCollection()
            .BindTo(this, x => x.Categories);
    }

    public UserId UserId { get; }

    public ReactiveCommand<Unit, HttpResponseMessage> Send { get; }

    public IEnumerable<CategorySelection> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    private IEnumerable<CategorySelection> _categories;
}