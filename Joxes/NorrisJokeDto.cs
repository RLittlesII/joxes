using Newtonsoft.Json;

namespace Joxes;

public class NorrisJokeDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("icon_url")]
    public Uri IconUrl { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("categories")]
    public IEnumerable<string> Categories { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}