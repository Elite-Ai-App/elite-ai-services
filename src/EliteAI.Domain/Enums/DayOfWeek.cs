using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TrainingDayOfWeek
{
    [JsonPropertyName("MONDAY")]
    MONDAY,
    [JsonPropertyName("TUESDAY")]
    TUESDAY,
    [JsonPropertyName("WEDNESDAY")]
    WEDNESDAY,
    [JsonPropertyName("THURSDAY")]
    THURSDAY,
    [JsonPropertyName("FRIDAY")]
    FRIDAY,
    [JsonPropertyName("SATURDAY")]
    SATURDAY,
    [JsonPropertyName("SUNDAY")]
    SUNDAY
} 