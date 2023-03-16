using FluentAssertions;
using Xunit;

namespace Joxes.Serialization;

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