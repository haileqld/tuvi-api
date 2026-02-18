using System.Text.Json.Serialization;

namespace TuviApi.Models;

public class InterpretationItem
{
    [JsonPropertyName("areaName")]
    public string AreaName { get; set; } = string.Empty;

    [JsonPropertyName("headline")]
    public string Headline { get; set; } = string.Empty;

    [JsonPropertyName("powerScore")]
    public int PowerScore { get; set; }

    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    [JsonPropertyName("advice")]
    public string Advice { get; set; } = string.Empty;
}
