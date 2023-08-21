using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Middlewares
{
    public class WebsocketMiddleware
    {
        private readonly RequestDelegate _next;
        
        public WebsocketMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Path.StartsWithSegments("/chatsocket", StringComparison.OrdinalIgnoreCase) &&
                    httpContext.Request.Query.TryGetValue("token", out var accessToken))
                {
                    httpContext.Request.Headers.Add("Authorization", $"Bearer {accessToken}");

                }
            }
            catch(Exception ex)
            { }
            await _next(httpContext);
        }
    }
}
