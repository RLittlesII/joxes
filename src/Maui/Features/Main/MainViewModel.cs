using ReactiveMarbles.Mvvm;

namespace Joxes.Maui;

public class MainViewModel
{
    public MainViewModel(ICoreRegistration coreRegistration) { _coreRegistration = coreRegistration; }

    private readonly ICoreRegistration _coreRegistration;
}