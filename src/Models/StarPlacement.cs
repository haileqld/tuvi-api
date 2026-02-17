using System.Text.Json.Serialization;

namespace TuviApi.Models;

public class StarPlacement
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("brightness")]
    public string Brightness { get; set; } = string.Empty;
}
