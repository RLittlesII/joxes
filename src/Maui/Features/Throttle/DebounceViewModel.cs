using ReactiveMarbles.Extensions;
using ReactiveMarbles.Mvvm;

namespace Joxes.Maui.Features.Throttle;

public class DebounceViewModel : ViewModelBase
{
    private readonly IJokeReceiver _jokeReceiver;

    public DebounceViewModel(INavigationService navigationService, ICoreRegistration coreRegistration, IJokeReceiver jokeReceiver)
        : base(navigationService, coreRegistration)
    {
        jokeReceiver
            .Listen()
            .Subscribe(_ => { })
            .DisposeWith(CollectMe);
    }
}