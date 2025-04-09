using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExerciseLevel
{
    [JsonPropertyName("BEGINNER")]
    BEGINNER,
    [JsonPropertyName("INTERMEDIATE")]
    INTERMEDIATE,
    [JsonPropertyName("ADVANCED")]
    ADVANCED
} 