using LanguageExt;

namespace Joxes;

public class JokeResponse : Record<JokeResponse>
{
    public JokeResponse(CorrelationId id, UserId userId, NorrisJokeDto jokeDto, DateTimeOffset timestamp)
    {
        Id = id;
        UserId = userId;
        JokeDto = jokeDto;
        Timestamp = timestamp;
    }

    public CorrelationId Id;

    public UserId UserId;

    public NorrisJokeDto JokeDto;

    public DateTimeOffset Timestamp;
}