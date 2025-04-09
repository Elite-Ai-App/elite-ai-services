using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Goal
{
    [JsonPropertyName("CARDIO")]
    CARDIO,
    [JsonPropertyName("CORE")]
    CORE,
    [JsonPropertyName("STRENGTH")]
    STRENGTH,
    [JsonPropertyName("SPEED")]
    SPEED,
    [JsonPropertyName("LATERAL_QUICKNESS")]
    LATERAL_QUICKNESS,
    [JsonPropertyName("VERTICAL_JUMP")]
    VERTICAL_JUMP
} 