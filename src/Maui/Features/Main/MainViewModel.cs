using System.Reactive;
using System.Reactive.Linq;
using ReactiveMarbles.Mvvm;
using ReactiveUI;

namespace Joxes.Maui.Features.Main;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(INavigationService navigationService, ICoreRegistration coreRegistration)
        : base(navigationService, coreRegistration)
    {
        Navigate = ReactiveCommand.CreateFromTask<string>(async uri => await NavigationService.NavigateAsync(uri)
                                                                          .HandleResult(),
                                                          Observable.Return(true));
    }

    public ReactiveCommand<string, Unit> Navigate { get; }
}