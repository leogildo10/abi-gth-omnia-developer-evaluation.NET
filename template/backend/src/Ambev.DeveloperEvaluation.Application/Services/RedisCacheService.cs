using Ambev.DeveloperEvaluation.Domain.Interfaces.Services;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Application.Services;

/// <summary>
/// Service for interacting with Redis cache.
/// </summary>
/// <remarks>
/// This service provides methods for setting, getting, and removing cache entries in Redis.
/// </remarks>
public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _redis;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
    /// </summary>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    /// <summary>
    /// Sets a value in the Redis cache.
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="value">The value to be cached.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetAsync(string key, string value)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// Gets a value from the Redis cache.
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value if found; otherwise, null.</returns>
    public async Task<string?> GetAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    /// <summary>
    /// Removes a value from the Redis cache.
    /// </summary>
    /// <param name="key">The key of the cache entry to be removed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
}
