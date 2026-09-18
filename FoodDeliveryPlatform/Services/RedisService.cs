using System.Text.Json;
using FDP.Interface;
using StackExchange.Redis;

namespace FDP.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private static readonly JsonSerializerOptions jsonOptions=new (JsonSerializerDefaults.Web);


    public RedisService(ConnectionMultiplexer database)
    {
        _database=database.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value,TimeSpan? expiry = null)
    {
        string json=JsonSerializer.Serialize(value,jsonOptions);

        if (expiry.HasValue)
        {
            await _database.StringSetAsync(key,json,new Expiration(expiry.Value));
        }
        else
        {
            await _database.StringSetAsync(key,json);
        }
    }

    public async Task <T?> GetAsync<T>(string key)
    {
        var value=await _database.StringGetAsync(key);

        if (!value.HasValue)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(value.ToString(),jsonOptions);
    }

    public async Task <bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        var endpoints=_database.Multiplexer.GetEndPoints();

        foreach(var endpoint in endpoints)
        {
            var server=_database.Multiplexer.GetServer(endpoint);

            var keys=server.Keys(pattern:pattern);

            foreach(var key in keys)
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }
}