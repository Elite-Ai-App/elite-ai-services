using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MuscleGroup
{
    [JsonPropertyName("CHEST")]
    CHEST,
    [JsonPropertyName("SHOULDERS")]
    SHOULDERS,
    [JsonPropertyName("TRICEPS")]
    TRICEPS,
    [JsonPropertyName("BICEPS")]
    BICEPS,
    [JsonPropertyName("BACK")]
    BACK,
    [JsonPropertyName("UPPER_BACK")]
    UPPER_BACK,
    [JsonPropertyName("LOWER_BACK")]
    LOWER_BACK,
    [JsonPropertyName("CORE")]
    CORE,
    [JsonPropertyName("OBLIQUES")]
    OBLIQUES,
    [JsonPropertyName("QUADRICEPS")]
    QUADRICEPS,
    [JsonPropertyName("HAMSTRINGS")]
    HAMSTRINGS,
    [JsonPropertyName("GLUTES")]
    GLUTES,
    [JsonPropertyName("CALVES")]
    CALVES,
    [JsonPropertyName("TIBIALIS")]
    TIBIALIS,
    [JsonPropertyName("ADDUCTORS")]
    ADDUCTORS,
    [JsonPropertyName("HIP_FLEXORS")]
    HIP_FLEXORS,
    [JsonPropertyName("FOREARMS")]
    FOREARMS,
    [JsonPropertyName("THORACIC_SPINE")]
    THORACIC_SPINE,
    [JsonPropertyName("ANKLES")]
    ANKLES,
    [JsonPropertyName("FULL_BODY")]
    FULL_BODY
}