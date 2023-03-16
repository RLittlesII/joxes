using Microsoft.Extensions.Logging;

namespace Joxes.Maui;

public static class PrismNavigationExtensions
{
    public static Task HandleResult(this Task<INavigationResult> task) =>
        HandleResult(task, result => Console.WriteLine(result.Exception));

    public static Task HandleResult(this Task<INavigationResult> task, Action<INavigationResult> handler) =>
        task.ContinueWith(
            continuation =>
            {
                if (!continuation.Result.Success)
                {
                    handler.Invoke(continuation.Result);
                }
            }
        );

    public static Action<NavigationRequestContext> HandleNavigationRequestContext(IContainerProvider provider) =>
        navigationRequestContext =>
        {
            var logger = provider.Resolve<ILogger<NavigationRequestContext>>();

            if (navigationRequestContext.Type == NavigationRequestType.Navigate)
            {
                logger.LogInformation("Navigation: {Uri}", navigationRequestContext.Uri);
            }

            else
            {
                logger.LogInformation("Navigation: {RequestType}", navigationRequestContext.Type);
            }

            var status = navigationRequestContext.Cancelled
                             ? "Cancelled"
                             : navigationRequestContext.Result.Success
                                 ? "Success"
                                 : "Failed";
            logger.LogInformation("Result: {Status}", status);

            if (status != "Failed"
             || string.IsNullOrEmpty(navigationRequestContext.Result?.Exception?.Message))
                return;

            var exception = navigationRequestContext.Result.Exception;
            logger.LogError(exception, exception.Message);
        };
}