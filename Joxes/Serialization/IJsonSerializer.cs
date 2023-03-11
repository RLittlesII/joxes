using System.Text.Json;
using System.Text.Json.Serialization;

namespace Joxes;

public interface IJsonSerializer
{
    string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);

    private static JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
                                                   {
                                                       IgnoreReadOnlyProperties = false,
                                                       IgnoreReadOnlyFields = false,
                                                       IncludeFields = true,
                                                       PropertyNameCaseInsensitive = true,
                                                       ReferenceHandler =
                                                           ReferenceHandler.IgnoreCycles,
                                                       Converters = { new NewTypeJsonConverterFactory() }
                                                   };
}