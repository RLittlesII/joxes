using Rocket.Surgery.Extensions.Testing.Fixtures;

namespace Joxes.Serialization;

internal sealed class SerializerFixture : ITestFixtureBuilder
{
    public static implicit operator Serializer(SerializerFixture fixture) => fixture.Build();
    public IJsonSerializer AsInterface() => Build();
    private Serializer Build() => new Serializer();
}