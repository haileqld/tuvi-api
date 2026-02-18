using System.Text.Json.Serialization;

namespace TuviApi.Models;

/// <summary>
/// A generic envelope for successful API responses, ensuring a consistent contract.
/// </summary>
/// <typeparam name="T">The type of the data payload.</typeparam>
public class SuccessEnvelope<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; } = true;

    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("error")]
    public object? Error { get; set; } = null;
}

/// <summary>
/// A minimal, RFC 7807-compliant problem details object for error responses.
/// </summary>
public class ProblemDetails
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    [JsonPropertyName("instance")]
    public string? Instance { get; set; }
}
