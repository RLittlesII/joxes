using DynamicData;
using Rocket.Surgery.Airframe.Data;

namespace Joxes.Blazor.Pages.Chuck;

public interface IChuckNorrisJokes : IChuckNorrisJokeService
{
    IObservable<IChangeSet<Category, string>> Categories();
}