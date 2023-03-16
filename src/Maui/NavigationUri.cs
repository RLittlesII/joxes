using Joxes.Maui.Features.Buffer;
using Joxes.Maui.Features.Discriminator;
using Joxes.Maui.Features.Main;
using Joxes.Maui.Features.Throttle;

namespace Joxes.Maui;

public static class NavigationUri
{
    public static string MainView => $"//{NavigationPage}/{nameof(MainPage)}";

    public static string Buffer => nameof(BufferScreen);
    public static string Debounce => nameof(DebounceScreen);
    public static string Discriminator => nameof(DiscriminatorScreen);
    public static string NavigationPage => nameof(NavigationPage);
}