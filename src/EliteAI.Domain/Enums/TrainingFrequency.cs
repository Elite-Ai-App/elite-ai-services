using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the frequency of training sessions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TrainingFrequency
{
    [JsonPropertyName("ONE_DAY_PER_WEEK")]
    ONE_DAY_PER_WEEK,
    [JsonPropertyName("TWO_DAYS_PER_WEEK")]
    TWO_DAYS_PER_WEEK,
    [JsonPropertyName("THREE_DAYS_PER_WEEK")]
    THREE_DAYS_PER_WEEK,
    [JsonPropertyName("FOUR_DAYS_PER_WEEK")]
    FOUR_DAYS_PER_WEEK,
    [JsonPropertyName("FIVE_DAYS_PER_WEEK")]
    FIVE_DAYS_PER_WEEK,
    [JsonPropertyName("SIX_DAYS_PER_WEEK")]
    SIX_DAYS_PER_WEEK
} 