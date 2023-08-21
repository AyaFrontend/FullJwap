using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Services
{
    public interface IMailService
    {
        public Task SendEmailAsync(string from , string to , string subject , string body);
        public int GenerateRandom4DigitsCode();
        //public Task CacheValidationCodeAsync(string key, string code, TimeSpan time);
        //public Task<string> GetCacheValidationCodeAsync(string key);
       
    }
}
