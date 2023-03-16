using LanguageExt;

namespace Joxes;

public class Joke : Record<Joke>
{
    public Joke(CorrelationId id,
                UserId userId,
                JokeId jokeId,
                Uri iconUrl,
                Punchline punchline,
                IEnumerable<Category> categories,
                DateTimeOffset timestamp)
    {
        Id = id;
        UserId = userId;
        JokeId = jokeId;
        IconUrl = iconUrl;
        Punchline = punchline;
        Category = categories.First();
        Timestamp = timestamp;
    }

    public CorrelationId Id { get; }
    public UserId UserId { get; }
    public JokeId JokeId { get; }
    public Punchline Punchline { get; }
    public Category Category { get; }
    public Uri IconUrl { get; set; }

    public DateTimeOffset Timestamp { get; set; }
}