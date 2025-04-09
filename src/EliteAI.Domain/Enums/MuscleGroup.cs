using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MuscleGroup
{
    [JsonPropertyName("CHEST")]
    CHEST,
    [JsonPropertyName("SHOULDERS")]
    SHOULDERS,
    [JsonPropertyName("TRICEPS")]
    TRICEPS
} 