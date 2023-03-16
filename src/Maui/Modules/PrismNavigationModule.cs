using Joxes.Maui.Features.Discriminator;
using Joxes.Maui.Features.Group;
using Joxes.Maui.Features.Main;
using Joxes.Maui.Features.Throttle;
using Rocket.Surgery.Airframe.Microsoft.Extensions.DependencyInjection;

namespace Joxes.Maui.Modules;

public class PrismNavigationModule : ServiceCollectionModule
{
    /// <inheritdoc />
    protected override IServiceCollection Load(IServiceCollection serviceCollection) =>
        serviceCollection
            .RegisterForNavigation<NavigationPage>(nameof(NavigationPage))
            .RegisterForNavigation<GroupScreen, GroupViewModel>(nameof(GroupScreen))
            .RegisterForNavigation<DiscriminatorScreen, DiscriminatorViewModel>(nameof(DiscriminatorScreen))
            .RegisterForNavigation<DebounceScreen, DebounceViewModel>(nameof(DebounceScreen))
            .RegisterForNavigation<MainPage, MainViewModel>(nameof(MainPage));
}