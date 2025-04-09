using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InjuryStatus
{
    [JsonPropertyName("ACTIVE")]
    ACTIVE,
    [JsonPropertyName("RECOVERING")]
    RECOVERING,
    [JsonPropertyName("RECOVERED")]
    RECOVERED,
    [JsonPropertyName("CHRONIC")]
    CHRONIC,
    [JsonPropertyName("RETIRED")]
    RETIRED
} 