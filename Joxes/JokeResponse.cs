using LanguageExt;

namespace Joxes;

public class JokeResponse : Record<JokeResponse>
{
    public JokeResponse(CorrelationId id, UserId userId, NorrisJoke joke, DateTimeOffset timestamp)
    {
        Id = id;
        UserId = userId;
        Joke = joke;
        Timestamp = timestamp;
    }

    public CorrelationId Id;

    public UserId UserId;

    public NorrisJoke Joke;

    public DateTimeOffset Timestamp;
}