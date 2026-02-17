using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace TuviApi.Models;

public class HoroscopeGenerateResponse
{
    [JsonPropertyName("interpretation")]
    public IEnumerable<InterpretationItem> Interpretation { get; set; } = new List<InterpretationItem>();

    [JsonPropertyName("technicalChart")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TechnicalChart? TechnicalChart { get; set; }
}
