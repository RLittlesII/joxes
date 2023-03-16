namespace Joxes.Maui;

public class GlobalReactiveExceptionHandler : IObserver<Exception>
{
    public static GlobalReactiveExceptionHandler Instance = __instance ??= new GlobalReactiveExceptionHandler();
    public void OnCompleted() { }
    public void OnError(Exception error) { }
    public void OnNext(Exception value) { }

    private static readonly GlobalReactiveExceptionHandler __instance;
}