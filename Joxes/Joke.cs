using LanguageExt;

namespace Joxes;

public class Joke : Record<Joke>
{
    public Joke(string jokeId, string punchLine, IEnumerable<string> categories)
    {
        Id = Guid.NewGuid();
        JokeId = new JokeId(jokeId);
        PunchLine = punchLine;
        Categories = categories.Select(category => new Category(category))
                               .ToList()
                               .AsReadOnly();
    }

    public Guid Id { get; }
    public JokeId JokeId { get; }
    public string PunchLine { get; }
    public IReadOnlyList<Category> Categories { get; }
}