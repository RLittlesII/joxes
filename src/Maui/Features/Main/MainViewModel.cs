using System.Reactive;
using ReactiveMarbles.Mvvm;
using ReactiveUI;

namespace Joxes.Maui.Features.Main;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(INavigationService navigationService, ICoreRegistration coreRegistration)
        : base(navigationService, coreRegistration)
    {
        Navigate = ReactiveCommand.CreateFromTask<string>(uri => NavigationService.NavigateAsync(uri)
                                                                               .HandleResult());
    }

    public ReactiveCommand<string, Unit> Navigate { get; }
}