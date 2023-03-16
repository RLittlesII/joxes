using Refit;

namespace Joxes;

public interface IChuckNorrisApiContract
{
    [Get("/jokes/random")]
    Task<NorrisJokeDto> Random();

    [Get("/jokes/random?category={category}")]
    Task<NorrisJokeDto> RandomFromCategory(string category);

    [Get("/jokes/categories")]
    Task<IEnumerable<string>> Categories();

    [Get("/jokes/search?query={query}")]
    Task<IEnumerable<NorrisJokeDto>> Search(string query);
}