using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InjurySeverity
{
    [JsonPropertyName("MINOR")]
    MINOR,
    [JsonPropertyName("MODERATE")]
    MODERATE,
    [JsonPropertyName("SEVERE")]
    SEVERE,
    [JsonPropertyName("CRITICAL")]
    CRITICAL
} 