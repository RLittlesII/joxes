namespace Joxes.Maui.Features;

public interface IChuckNorrisApiClient
{
    IObservable<JokeResponse> RandomFromCategory(params Category[] categories);
}