using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the type of exercise.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExerciseType
{
    [JsonPropertyName("STRENGTH")]
    STRENGTH,
    [JsonPropertyName("MOBILITY")]
    MOBILITY
} 