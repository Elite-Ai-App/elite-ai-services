using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InjuryType
{
    [JsonPropertyName("MUSCLE_STRAIN")]
    MUSCLE_STRAIN,
    [JsonPropertyName("LIGAMENT_SPRAIN")]
    LIGAMENT_SPRAIN,
    [JsonPropertyName("TENDONITIS")]
    TENDONITIS,
    [JsonPropertyName("FRACTURE")]
    FRACTURE,
    [JsonPropertyName("DISLOCATION")]
    DISLOCATION,
    [JsonPropertyName("CONCUSSION")]
    CONCUSSION,
    [JsonPropertyName("CONTUSION")]
    CONTUSION,
    [JsonPropertyName("OVERUSE")]
    OVERUSE,
    [JsonPropertyName("CHRONIC")]
    CHRONIC,
    [JsonPropertyName("OTHER")]
    OTHER
} 