using System.Collections.Concurrent;
using System.Collections.Specialized;

namespace UrlShortener.ReadApi.Services;

/// <summary>
/// A simple in-memory LRU (Least Recently Used) cache for storing URL mappings
/// </summary>
public class UrlCache : IUrlCache
{
    private readonly int _capacity = 1000;
    private readonly ConcurrentDictionary<string, string> _cache = new();
    private readonly OrderedDictionary _lruTracker = new();

    public UrlCache()
    {
    }

    public string? Get(string shortUrl)
    {
        if (_cache.TryGetValue(shortUrl, out var longUrl))
        {
            // Update access order for LRU tracking
            lock (_lruTracker)
            {
                _lruTracker.Remove(shortUrl);
                _lruTracker.Add(shortUrl, null);
            }
            return longUrl;
        }
        
        return null;
    }

    public void Set(string shortUrl, string longUrl)
    {
        // Add or update the cache
        _cache[shortUrl] = longUrl;
        
        lock (_lruTracker)
        {
            // Update LRU tracking
            if (_lruTracker.Contains(shortUrl))
            {
                _lruTracker.Remove(shortUrl);
            }
            
            // Add to the end to mark as most recently used
            _lruTracker.Add(shortUrl, null);
            
            // If over capacity, remove least recently used item
            if (_lruTracker.Count > _capacity)
            {
                var lruKey = _lruTracker.Cast<System.Collections.DictionaryEntry>().First().Key.ToString();
                if (lruKey != null)
                {
                    _lruTracker.Remove(lruKey);
                    _cache.TryRemove(lruKey, out _);
                }
            }
        }
    }
}
