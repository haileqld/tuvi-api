using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TuviApi.Models;

public class TechnicalChart
{
    [JsonPropertyName("palaces")]
    public IEnumerable<Palace> Palaces { get; set; } = new List<Palace>();
}
