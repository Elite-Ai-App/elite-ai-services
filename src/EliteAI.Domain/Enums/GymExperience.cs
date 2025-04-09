using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the gym experience level of a user.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GymExperience
{
    /// <summary>
    /// No prior gym experience
    /// </summary>
    [JsonPropertyName("BEGINNER")]
    BEGINNER,

    /// <summary>
    /// Some gym experience (1-6 months)
    /// </summary>
    [JsonPropertyName("INTERMEDIATE")]
    INTERMEDIATE,

    /// <summary>
    /// Significant gym experience (6+ months)
    /// </summary>
    [JsonPropertyName("ADVANCED")]
    ADVANCED,

    /// <summary>
    /// Professional or competitive level experience
    /// </summary>
    Professional
} 