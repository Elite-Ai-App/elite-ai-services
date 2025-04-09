using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Position
{
    [JsonPropertyName("POINT_GUARD")]
    POINT_GUARD,
    [JsonPropertyName("SHOOTING_GUARD")]
    SHOOTING_GUARD,
    [JsonPropertyName("SMALL_FORWARD")]
    SMALL_FORWARD,
    [JsonPropertyName("POWER_FORWARD")]
    POWER_FORWARD,
    [JsonPropertyName("CENTER")]
    CENTER
} 