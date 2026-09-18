using StackExchange.Redis;

namespace FDP.Interface;

public interface IRedisService
{
    Task SetAsync<T>(string key,T value, TimeSpan? expiry=null);
    Task<T?> GetAsync<T>(string key);
    Task<bool> DeleteAsync(string key);
    Task<bool> ExistsAsync(string key);

    Task RemoveByPatternAsync(string pattern);

} 