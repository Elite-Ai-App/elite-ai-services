using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GymAccess
{
    [JsonPropertyName("FULL_ACCESS")]
    FULL_ACCESS,
    [JsonPropertyName("LIMITED_ACCESS")]
    LIMITED_ACCESS,
    [JsonPropertyName("NO_ACCESS")]
    NO_ACCESS
} 