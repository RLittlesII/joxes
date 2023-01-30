using ReactiveUI;

namespace Joxes.Maui.Features;

public class ContentPageBase<TViewModel> : ContentPage, IViewFor<TViewModel>
    where TViewModel : class
{
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel) value;
    }

    public TViewModel ViewModel { get; set; }
}