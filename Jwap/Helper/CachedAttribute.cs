using Jwap.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.API.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private int _time;
       
        public CachedAttribute(int time)
        {
            _time = time;
            
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string key = generateKeyFromRequest(context.HttpContext);
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheRepository>();
            var response = await cacheService.GetCachedResponse(key);
            if (!string.IsNullOrEmpty(response))
            {

                var contentResult = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            await next();
            if (context.Result is OkObjectResult okObjectResult)
                await cacheService.CacheResponseAsync(key, okObjectResult.Value, TimeSpan.FromSeconds(_time));
        }

        public string generateKeyFromRequest(HttpContext httpContext)
        {
            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append($"{httpContext.Request.Path}");
            foreach (var (key , value) in httpContext.Request.Query)
            {
                keyBuilder.Append($"{key}-{value}|");
            }
            return keyBuilder.ToString();
        }
    }
}
