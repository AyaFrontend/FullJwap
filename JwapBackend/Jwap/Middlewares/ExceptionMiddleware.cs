using Jwap.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwap.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next { set; get; }
        private ILogger<ExceptionMiddleware> _logger { set; get; }
        private IHostEnvironment _env { set; get; }


        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
                                   IHostEnvironment env)
        {
            this._next = next;
            this._logger = logger;
            this._env = env; 
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(System.Exception ex)
            {
                _logger.LogError(ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
                var responseMessage = _env.IsDevelopment() ? new ApiExceptionErrorResponse((int)
                                     HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                                     new ApiExceptionErrorResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(responseMessage , options)) ;

            }
        }
    }
}
