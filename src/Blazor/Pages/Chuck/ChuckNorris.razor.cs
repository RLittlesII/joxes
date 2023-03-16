using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;

namespace Joxes.Blazor.Pages.Chuck;

public partial class ChuckNorris
{
    public ChuckNorris() =>
        this.WhenAnyObservable(x => x.ViewModel.Changed)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Subscribe(_ => InvokeAsync(StateHasChanged));

    protected override void OnInitialized() => ViewModel = _viewModel;

    private Task Send() =>
        InvokeAsync(() => ViewModel
                          .Send
                          .Execute()
                          .ToTask());
}