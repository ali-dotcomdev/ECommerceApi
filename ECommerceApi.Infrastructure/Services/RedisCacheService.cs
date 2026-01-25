using ECommerceApi.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ECommerceApi.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonString = await _cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(jsonString))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(60)
        };

        var jsonString = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(key, jsonString, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
