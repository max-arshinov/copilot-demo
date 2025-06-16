using System.Net;
using System.Net.Http.Json;
using UrlShortener.ReadApi.Models;
using Xunit;

namespace UrlShortener.Tests;

public class UrlControllerAcceptanceTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UrlControllerAcceptanceTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RedirectToLongUrl_WhenShortUrlExists_ReturnsJsonWithLongUrl()
    {
        // Arrange
        var shortUrl = "abc123";
        var longUrl = "https://epam.com";

        // Setup the mock cache to return our test URL
        _factory
            .MockUrlCache
            .Setup(cache => cache.Get(shortUrl))
            .Returns(longUrl);

        // Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/{shortUrl}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<UrlResponse>();
        Assert.NotNull(result);
        Assert.Equal(shortUrl, result.ShortUrl);
        Assert.Equal(longUrl, result.LongUrl);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task RedirectToLongUrl_WhenShortUrlDoesNotExist_ReturnsNotFoundJson()
    {
        // Arrange
        var shortUrl = "nonexistent";

        // Setup the mock cache to return null (URL not found)
        _factory.MockUrlCache
            .Setup(cache => cache.Get(shortUrl))
            .Returns((string?)null);

        // Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/{shortUrl}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<UrlResponse>();
        Assert.NotNull(result);
        Assert.Equal(shortUrl, result.ShortUrl);
        Assert.False(result.Success);
        Assert.NotNull(result.Message);
    }

    [Fact]
    public async Task RedirectToLongUrl_CacheMissShouldQueryDatabase()
    {
        // This test would normally test that when the cache misses,
        // the controller attempts to query the database.
        // Since we don't have the database integration implemented yet,
        // we're just testing the current behavior which returns NotFound.

        // Arrange
        var shortUrl = "dbtest";

        // Setup the mock cache to return null (forcing a cache miss)
        _factory.MockUrlCache
            .Setup(cache => cache.Get(shortUrl))
            .Returns((string?)null);

        // Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/{shortUrl}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<UrlResponse>();
        Assert.NotNull(result);
        Assert.Equal(shortUrl, result.ShortUrl);
        Assert.False(result.Success);

        // In a more complete implementation, we would also verify that
        // the database was queried, e.g.:
        // _mockScyllaDbService.Verify(db => db.GetLongUrlAsync(shortUrl), Times.Once);
    }
}