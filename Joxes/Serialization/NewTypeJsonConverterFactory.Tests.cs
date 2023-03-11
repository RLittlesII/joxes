using FluentAssertions;
using Rocket.Surgery.Extensions.Testing.Fixtures;
using Xunit;

namespace Joxes;

public class NewTypeJsonConverterFactoryTests
{
    [Fact]
    public void Given_When_Then()
    {
        // Given
        var correlationId = new CorrelationId();

        // When
        var result = new SerializerFixture().AsInterface().Serialize(correlationId);

        // Then
        result
            .Should()
            .NotBeNull();
    }
}

internal sealed class SerializerFixture : ITestFixtureBuilder
{
    public static implicit operator Serializer(SerializerFixture fixture) => fixture.Build();
    public IJsonSerializer AsInterface() => Build();
    private Serializer Build() => new Serializer();
}