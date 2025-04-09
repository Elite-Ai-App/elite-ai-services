using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InjuryArea
{
    [JsonPropertyName("ANKLE")]
    ANKLE,
    [JsonPropertyName("CHEST")]
    CHEST,
    [JsonPropertyName("SHOULDERS")]
    SHOULDERS,
    [JsonPropertyName("TRICEPS")]
    TRICEPS,
    [JsonPropertyName("FOREARMS")]
    FOREARMS,
    [JsonPropertyName("HAMSTRING")]
    HAMSTRING,
    [JsonPropertyName("GLUTE")]
    GLUTE,
    [JsonPropertyName("QUADRICEPS")]
    QUADRICEPS,
    [JsonPropertyName("CALVES")]
    CALVES,
    [JsonPropertyName("OTHER")]
    OTHER
} 