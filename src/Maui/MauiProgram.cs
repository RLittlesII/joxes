using ReactiveMarbles.Mvvm;
using ReactiveUI;

namespace Joxes.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseShiny(collection => collection.AddConnectivity())
            .UseServices(collection => collection
                                       .AddGlobalExceptionHandler<GlobalReactiveExceptionHandler>()
                                       .AddCoreRegistration(provider =>
                                                                CoreRegistrationBuilder
                                                                    .Create()
                                                                    .WithExceptionHandler(
                                                                        provider.GetService<IObserver<Exception>>())
                                                                    .WithMainThreadScheduler(RxApp.MainThreadScheduler)
                                                                    .WithTaskPoolScheduler(RxApp.TaskpoolScheduler)
                                                                    .Build()))
            .UseServices(collection => collection
                             .AddNavigation<MainPage, MainViewModel>())
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}

public static class MarblesExtensions
{
    public static MauiAppBuilder UseShiny(this MauiAppBuilder builder, Action<IServiceCollection> configuration)
    {
        builder.UseShiny();
        configuration.Invoke(builder.Services);
        return builder;
    }

    public static MauiAppBuilder UseServices(this MauiAppBuilder builder, Action<IServiceCollection> configuration)
    {
        configuration.Invoke(builder.Services);
        return builder;
    }

    public static IServiceCollection AddGlobalExceptionHandler(
        this IServiceCollection services,
        Func<IServiceProvider, IObserver<Exception>> implementationFactory) =>
        services.AddSingleton(typeof(IObserver<Exception>), implementationFactory);

    public static IServiceCollection AddGlobalExceptionHandler<THandler>(this IServiceCollection services)
        where THandler : IObserver<Exception> =>
        services.AddSingleton(typeof(IObserver<Exception>), typeof(THandler));

    public static IServiceCollection AddCoreRegistration(
        this IServiceCollection services,
        Func<IServiceProvider, ICoreRegistration> implementationFactory)
    {
        return services.AddSingleton(typeof(ICoreRegistration), implementationFactory);
    }

    public static IServiceCollection AddNavigation<TPage, TViewModel>(this IServiceCollection serviceCollection)
        where TViewModel : class
        where TPage : class =>
        serviceCollection
            .AddTransient<TPage>()
            .AddTransient<TViewModel>();
}

public class GlobalReactiveExceptionHandler : IObserver<Exception>
{
    public static GlobalReactiveExceptionHandler Instance = __instance ??= new GlobalReactiveExceptionHandler();
    private static readonly GlobalReactiveExceptionHandler __instance;
    public void OnCompleted() { }
    public void OnError(Exception error) { }
    public void OnNext(Exception value) { }
}