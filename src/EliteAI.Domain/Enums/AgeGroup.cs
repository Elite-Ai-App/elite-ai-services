using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the age group of a user.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AgeGroup
{
    [JsonPropertyName("JUNIOR")]
    JUNIOR,
    [JsonPropertyName("TEEN")]
    TEEN,
    [JsonPropertyName("YOUNG_ADULT")]
    YOUNG_ADULT,
    [JsonPropertyName("ADULT")]
    ADULT
} 