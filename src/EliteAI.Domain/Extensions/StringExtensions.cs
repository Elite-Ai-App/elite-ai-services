using System.Text.Json;

namespace EliteAI.Domain.Extensions;

public static class StringExtensions
{
    public static string ToJson(this string str)
    {
        try
        {
            // Try to parse and re-serialize to ensure valid JSON
            using var doc = JsonDocument.Parse(str);
            return JsonSerializer.Serialize(doc.RootElement);
        }
        catch
        {
            // If parsing fails, return the original string
            return str;
        }
    }
} 