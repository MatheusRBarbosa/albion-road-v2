using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;

using AlbionRoad.Domain.Models;
using AlbionRoad.Domain.Interfaces.Services;
using AlbionRoad.Resources.Configs;

namespace AlbionRoad.Infra.Services.Cache;
public class RedisService : ICacheService
{
    private readonly string defaultKey = "raw";
    private readonly IDistributedCache cache;
    private readonly RedisSettings redisSettings;

    public RedisService(
        IDistributedCache cache,
        IOptions<RedisSettings> redisSettings
    )
    {
        this.cache = cache;
        this.redisSettings = redisSettings.Value;
    }

    public async Task<IList<Price>?> GetPricesAsync()
    {
        var rawValue = await cache.GetStringAsync(defaultKey);
        if (rawValue == null)
        {
            return null;
        }

        var list = JsonSerializer.Deserialize<List<Price>>(rawValue);
        return list;
    }

    public async Task<IList<Profit>?> GetProfitsAsync(Route route)
    {
        var key1 = $"{route.From.Id}-{route.To.Id}";
        var key2 = $"{route.To.Id}-{route.From.Id}";

        var rawValue = await cache.GetStringAsync(key1);
        if (rawValue == null)
        {
            rawValue = await cache.GetStringAsync(key2);
        }

        if (rawValue == null)
        {
            return null;
        }

        var list = JsonSerializer.Deserialize<List<Profit>>(rawValue);

        return list;
    }

    public async void SetRawPrices(IList<Price> items)
    {
        if (items.Count > 0)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(redisSettings.TTL)
            };

            var key = defaultKey;
            var value = JsonSerializer.Serialize(items);
            await cache.SetStringAsync(key, value, options);
        }
    }

    public async void SetItemsProfit(Route route, IList<Profit> profits)
    {
        if (profits.Count > 0)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(redisSettings.TTL)
            };

            var key = $"{route.From.Id}-{route.To.Id}";
            var value = JsonSerializer.Serialize(profits);

            await cache.SetStringAsync(key, value, options);
        }
    }
}