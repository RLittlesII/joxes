using System.Reactive.Linq;
using ReactiveUI;

namespace Blazor.Pages;

public partial class ChuckNorris
{
    public ChuckNorris() =>
        this.WhenAnyObservable(x => x.ViewModel.Changed)
            .Throttle(TimeSpan.FromMilliseconds(500), RxApp.MainThreadScheduler)
            .Subscribe(_ => StateHasChanged());

    protected override void OnInitialized() => ViewModel = _viewModel;
}