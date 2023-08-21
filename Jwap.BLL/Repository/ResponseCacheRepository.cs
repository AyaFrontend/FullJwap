using Jwap.BLL.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwap.BLL.Repository
{
    public class ResponseCacheRepository : IResponseCacheRepository
    {
        private IDatabase _database;
        public ResponseCacheRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string key, object response, TimeSpan time)
        {
            if (response == null) return;

            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive=false };
            var responsAsJson = JsonSerializer.Serialize(response, option);
            await _database.StringSetAsync(key, responsAsJson, time);
        }

        public async Task<string> GetCachedResponse(string key)
        {
     

            var chachedResponse = await _database.StringGetAsync(key);
            return chachedResponse; 
        }
    }
}
