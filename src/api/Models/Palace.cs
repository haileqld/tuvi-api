using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TuviApi.Models;

public class Palace
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("stars")]
    public IEnumerable<StarPlacement> Stars { get; set; } = new List<StarPlacement>();
}
