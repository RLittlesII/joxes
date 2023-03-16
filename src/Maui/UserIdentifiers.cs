namespace Joxes.Maui;

public class UserIdentifiers
{
    public static UserId GetRandom() =>
        UserIds.OrderBy(_ => Randomizer.Random.Next())
               .First();


    private static readonly IReadOnlyList<UserId> UserIds = Enumerable.Repeat(new UserId(), 10)
                                                                      .ToList()
                                                                      .AsReadOnly();
}

public class Randomizer
{
    public static readonly Random Random = new();
}