namespace Joxes.Maui;

public class UserIdentifiers
{
    public static UserId GetRandom() =>
        UserIds.OrderBy(_ => Random.Next())
               .First();

    private static readonly Random Random = new();

    private static readonly IReadOnlyList<UserId> UserIds = Enumerable.Repeat(new UserId(), 10)
                                                                      .ToList()
                                                                      .AsReadOnly();
}