namespace Ambev.DeveloperEvaluation.Domain.Interfaces.Services;

/// <summary>
/// Interface for Redis cache service.
/// </summary>
/// <remarks>
/// This interface defines methods for setting, getting, and removing cache entries in Redis.
/// </remarks>
public interface IRedisCacheService
{
    /// <summary>
    /// Sets a value in the Redis cache.
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    /// <param name="value">The value to be cached.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetAsync(string key, string value);

    /// <summary>
    /// Gets a value from the Redis cache.
    /// </summary>
    /// <param name="key">The key of the cache entry.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value if found; otherwise, null.</returns>
    Task<string?> GetAsync(string key);

    /// <summary>
    /// Removes a value from the Redis cache.
    /// </summary>
    /// <param name="key">The key of the cache entry to be removed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(string key);
}
