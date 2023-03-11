using FluentAssertions;
using Xunit;

namespace Joxes;

public class SerializerTests
{
    [Fact]
    public void Given_When_Then()
    {
        // Given
        var jokeRequest = new JokeRequest(new UserId(), new List<Category>());

        // When
        var serialized = new SerializerFixture().AsInterface()
                                                .Serialize(jokeRequest);
        var result = new SerializerFixture().AsInterface()
                                            .Deserialize<JokeRequest>(serialized);

        // Then
        result
            .Should()
            .NotBeNull();

        result
            .Id
            .Should()
            .Be(jokeRequest.Id);

        result
            .UserId
            .Should()
            .Be(jokeRequest.UserId);
    }
}