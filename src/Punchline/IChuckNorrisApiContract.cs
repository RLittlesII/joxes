using Joxes;
using Refit;

namespace Punchline;

public interface IChuckNorrisApiContract
{
    [Get("/jokes/random")]
    Task<NorrisJoke> Random();

    [Get("/jokes/random?category={category}")]
    Task<NorrisJoke> RandomFromCategory(string category);

    [Get("/jokes/categories")]
    Task<IEnumerable<string>> Categories();

    [Get("/jokes/search?query={query}")]
    Task<IEnumerable<NorrisJoke>> Search(string query);
}