namespace UrlShortener.ReadApi.Models;

public class UrlMapping
{
    public string Id { get; set; } = string.Empty;
    public string LongUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastAccessed { get; set; }
    public int AccessCount { get; set; }
}
