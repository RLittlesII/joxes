using ReactiveMarbles.Mvvm;

namespace Joxes.Maui.Features.Discriminator;

public class DiscriminatorViewModel : ViewModelBase
{
    public DiscriminatorViewModel(INavigationService navigationService, ICoreRegistration coreRegistration)
        : base(navigationService, coreRegistration) { }
}