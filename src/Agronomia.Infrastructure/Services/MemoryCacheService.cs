using Microsoft.Extensions.Caching.Memory;
using Agronomia.Domain.Interfaces;

namespace Agronomia.Infrastructure.Services;

/// <summary>
/// Provides an implementation of <see cref="ICacheService"/> using an in-memory cache.
/// </summary>
/// <remarks>
/// <para>
/// This implementation delegates to <see cref="IMemoryCache"/>, leveraging in-process memory for low-latency caching.
/// Use it for single-node deployments or scenarios where cache entries do not need to be shared across processes.
/// </para>
/// <para>
/// Entries can optionally use an absolute expiration relative to the current time. Sliding expiration or size-based eviction
/// can be added via <see cref="MemoryCacheEntryOptions"/> if needed.
/// </para>
/// <para>
/// Example:
/// <code>
/// var cache = new MemoryCache(new MemoryCacheOptions());
/// var service = new MemoryCacheService(cache);
/// await service.SetAsync("movie:42", movie, TimeSpan.FromMinutes(5));
/// var cached = await service.GetAsync<Movie>("movie:42");
/// </code>
/// </para>
/// </remarks>
/// <param name="cache">The <see cref="IMemoryCache"/> instance used for caching.</param>
public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    private readonly IMemoryCache _cache = cache;

    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out T? value))
        {
            return Task.FromResult(value);
        }

        return Task.FromResult<T?>(default);
    }

    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (absoluteExpirationRelativeToNow.HasValue)
        {
            options.SetAbsoluteExpiration(absoluteExpirationRelativeToNow.Value);
        }

        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}
