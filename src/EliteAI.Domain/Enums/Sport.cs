using System.Text.Json.Serialization;

namespace EliteAI.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Sport
{
    [JsonPropertyName("BASKETBALL")]
    BASKETBALL
} 