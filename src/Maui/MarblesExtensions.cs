using ReactiveMarbles.Mvvm;

namespace Joxes.Maui;

public static class MarblesExtensions
{
    public static MauiAppBuilder UseMarbles(this MauiAppBuilder builder, Action<IServiceCollection> configuration)
    {
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