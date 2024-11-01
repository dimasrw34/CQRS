using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InTouch.Infrastructure;

internal class DistributedCashService(
    IDistributedCache distributedCache,
    ILogger<DistributedCashService> logger,
    IOptions<CacheOptions> cacheOptions) : ICacheService
{
    private const string CacheServiceName = nameof(DistributedCashService);

    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cacheOptions.Value.AbsoluteExpirationInHours), 
        SlidingExpiration = TimeSpan.FromSeconds(cacheOptions.Value.SlidingExpirationInSeconds)
    };
        
    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        var valueBytes = await distributedCache.GetAsync(cacheKey);

        if (valueBytes?.Length>0)
        {
            logger.LogInformation("\"----- Fetched from {{CacheServiceName}}: '{{CacheKey}}'\", CacheServiceName, cacheKey");
        }

        var item = await factory();
        if (!item.IsDefault()) // SonarQube Bug: item != nulll
        {
            logger.LogInformation("----- Added to {CacheServiceName}: '{CacheKey}'", CacheServiceName, cacheKey);

            var value = Encoding.UTF8.GetBytes(item.ToJson());
            await distributedCache.SetAsync(cacheKey, value, _cacheOptions);
        }

        return item;
    }

    public async Task<IReadOnlyList<TItem>> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<IReadOnlyList<TItem>>> factory)
    {
        var valueBytes = await distributedCache.GetAsync(cacheKey);
        if (valueBytes?.Length > 0)
        {
            logger.LogInformation("----- Fetched from {CacheServiceName}: '{CacheKey}'", CacheServiceName, cacheKey);
            var values = Encoding.UTF8.GetString(valueBytes);
            return values.FromJson<IReadOnlyList<TItem>>();
        }

        var items = await factory();
        if (items?.Any() == true)
        {
            logger.LogInformation("----- Added to {CacheServiceName}: '{CacheKey}'", CacheServiceName, cacheKey);
            var value = Encoding.UTF8.GetBytes(items.ToJson());
            await distributedCache.SetAsync(cacheKey, value, _cacheOptions);
        }

        return items;
    }

    public async Task RemoveAsync(params string[] cacheKeys)
    {
        foreach (var cacheKey in cacheKeys)
        {
            logger.LogInformation("----- Removed from {CacheServiceName}: '{CacheKey}'", CacheServiceName, cacheKey);
            await distributedCache.RefreshAsync(cacheKey);
        }
    }
}