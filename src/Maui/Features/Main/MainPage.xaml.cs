using ReactiveUI;

namespace Joxes.Maui.Features.Main;

public partial class MainPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();

        this.WhenActivated(disposables => { });
    }
}