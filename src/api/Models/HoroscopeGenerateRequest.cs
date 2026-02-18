using System.Text.Json.Serialization;

namespace TuviApi.Models;

public class HoroscopeGenerateRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("gregorianBirthDate")]
    public DateTime GregorianBirthDate { get; set; }

    [JsonPropertyName("timezoneOffset")]
    public double TimezoneOffset { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; } = "en";

    [JsonPropertyName("includeTechnicalDetails")]
    public bool IncludeTechnicalDetails { get; set; } = false;
}
