using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the type of measurement units used.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UnitType
{
    [JsonPropertyName("IMPERIAL")]
    /// <summary>
    /// Imperial units (pounds, feet, inches)
    /// </summary>
    IMPERIAL,

    [JsonPropertyName("METRIC")]
    /// <summary>
    /// Metric units (kilograms, meters, centimeters)
    /// </summary>
    METRIC
} 