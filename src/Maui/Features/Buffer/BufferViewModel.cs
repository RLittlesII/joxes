using ReactiveMarbles.Mvvm;

namespace Joxes.Maui.Features.Buffer;

public class BufferViewModel : ViewModelBase
{
    public BufferViewModel(INavigationService navigationService, ICoreRegistration coreRegistration)
        : base(navigationService, coreRegistration) { }
}