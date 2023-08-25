using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Interfaces
{
    public interface IResponseCacheRepository
    {
        public Task<string> GetCachedResponse(string key);
        public Task CacheResponseAsync(string key, object response , TimeSpan time );
    }
}
