namespace UrlShortener.ReadApi.Services;

public interface IUrlCache
{
    string? Get(string shortUrl);

    void Set(string shortUrl, string longUrl);
}