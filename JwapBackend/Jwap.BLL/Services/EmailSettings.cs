using Jwap.BLL.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Services
{
    public class EmailSettings: IMailService 
    {

        private readonly IDatabase _db;
        public EmailSettings(IConnectionMultiplexer connection)
        {
            _db = connection.GetDatabase();
        }


        //public async Task CacheValidationCodeAsync(string key, string code, TimeSpan time)
        //{
        //    if (code == null) return;

        //    await _db.StringSetAsync(key, code , time);
        //}
        //public async Task<string> GetCacheValidationCodeAsync(string key)
        //{
        //    return await _db.StringGetAsync(key);
        //}

        public int GenerateRandom4DigitsCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            var client = new SmtpClient("smtp-mail.outlook.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ayamohamedabdelrahman868@gmail.com", "AyaMohamed123456");

            try
            {
                await client.SendMailAsync(from, to, subject, body);
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }
        }
    }
}
