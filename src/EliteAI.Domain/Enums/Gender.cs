using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the gender of a user.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    [JsonPropertyName("MALE")]
    /// <summary>
    /// Male gender
    /// </summary>
    MALE,

    [JsonPropertyName("FEMALE")]
    /// <summary>
    /// Female gender
    /// </summary>
    FEMALE,

    [JsonPropertyName("NOT_SPECIFIED")]
    /// <summary>
    /// Other gender
    /// </summary>
    NOT_SPECIFIED
} 