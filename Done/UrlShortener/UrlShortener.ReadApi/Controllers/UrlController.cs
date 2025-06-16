using Microsoft.AspNetCore.Mvc;
using UrlShortener.ReadApi.Models;
using UrlShortener.ReadApi.Services;

namespace UrlShortener.ReadApi.Controllers;

[ApiController]
public class UrlController : ControllerBase
{
    private readonly ILogger<UrlController> _logger;
    private readonly IUrlCache _cache;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public UrlController(
        ILogger<UrlController> logger, 
        IUrlCache cache,
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _cache = cache;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [HttpGet("/{shortUrl}")]
    public async Task<IActionResult> RedirectToLongUrl(string shortUrl)
    {
        try
        {
            // First check the cache
            var longUrl = _cache.Get(shortUrl);

            if (longUrl == null)
            {
                // Not in cache, need to query the database (ScyllaDB)
                // This is just a placeholder. You'd implement actual ScyllaDB query here
                // longUrl = await _scyllaDbService.GetLongUrlAsync(shortUrl);
                
                // For now, returning not found with a JSON response
                return NotFound(new UrlResponse 
                { 
                    ShortUrl = shortUrl,
                    Success = false,
                    Message = $"Short URL '{shortUrl}' not found"
                });
            }

            // Report access to hit counter service asynchronously (fire and forget)
            _ = Task.Run(async () => 
            {
                try
                {
                    // This is just a placeholder. You'd implement actual hit counter service call here
                    // await _httpClient.PostAsync("http://hitcounter/api/hit", 
                    //    new StringContent(JsonSerializer.Serialize(new { shortUrl })));
                }
                catch (Exception ex)
                {
                    // Log but don't fail the response if hit counter is down
                    _logger.LogError(ex, "Failed to record hit for {ShortUrl}", shortUrl);
                }
            });

            // Return a JSON response with the URL information
            return Ok(new UrlResponse
            {
                ShortUrl = shortUrl,
                LongUrl = longUrl,
                Success = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing {ShortUrl}", shortUrl);
            return StatusCode(500, new UrlResponse
            {
                ShortUrl = shortUrl,
                Success = false,
                Message = "An error occurred while processing the request"
            });
        }
    }
}
