using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the skill level in a sport.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SportLevel
{
    [JsonPropertyName("MIDDLE_SCHOOL")]
    MIDDLE_SCHOOL,
    [JsonPropertyName("HIGH_SCHOOL")]
    HIGH_SCHOOL,
    [JsonPropertyName("AMATEUR")]
    AMATEUR,
    [JsonPropertyName("COLLEGE")]
    COLLEGE,
    [JsonPropertyName("PROFESSIONAL")]
    PROFESSIONAL
} 