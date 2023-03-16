using DryIoc;
using Joxes.Maui.Modules;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using ReactiveMarbles.Mvvm;
using ReactiveUI;
using Rocket.Surgery.Airframe.Microsoft.Extensions.DependencyInjection;
using static Joxes.Maui.PrismNavigationExtensions;

namespace Joxes.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp() =>
        MauiApp
            .CreateBuilder()
            .UseMauiApp<App>()
            .UsePrism(new DryIocContainerExtension(Rules.MicrosoftDependencyInjectionRules),
                      prismAppBuilder =>
                          prismAppBuilder
                              .ConfigureServices(services =>
                                                     services
                                                         .AddModule<PrismNavigationModule>()
                                                         .AddSingleton<IJokeReceiver, JokeReceiver>()
                                                         .AddSingleton<IHubConnectionBuilder>(
                                                             _ => new HubConnectionBuilder().WithUrl(
                                                                 "https://punchline.service.signalr.net/jokes"))
                                                         .RegisterGlobalNavigationObserver()
                                                         .AddGlobalExceptionHandler<GlobalReactiveExceptionHandler>()
                                                         .AddCoreRegistration(provider =>
                                                                                  CoreRegistrationBuilder
                                                                                      .Create()
                                                                                      .WithExceptionHandler(
                                                                                          provider
                                                                                              .GetService<IObserver<Exception>>())
                                                                                      .WithMainThreadScheduler(
                                                                                          RxApp.MainThreadScheduler)
                                                                                      .WithTaskPoolScheduler(RxApp.TaskpoolScheduler)
                                                                                      .Build())
                                                         .AddLogging(configure => configure.AddConsole()))
                              .OnAppStart((_, navigation) => navigation.NavigateAsync(NavigationUri.MainView)
                                                                       .HandleResult())
                              .AddGlobalNavigationObserver((provider, context) =>
                                                               context.Subscribe(
                                                                   HandleNavigationRequestContext(
                                                                       provider))))
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .Build();
}