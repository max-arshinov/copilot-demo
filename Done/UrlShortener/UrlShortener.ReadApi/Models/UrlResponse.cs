namespace UrlShortener.ReadApi.Models;

public class UrlResponse
{
    public string ShortUrl { get; set; } = string.Empty;
    public string LongUrl { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? Message { get; set; }
}
