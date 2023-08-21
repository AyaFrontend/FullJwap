using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { set; get; }
        public string MessageError { set; get; } = null;

        public ApiResponse(int statusCode , string message = null)
        {
            StatusCode = statusCode;
            MessageError = message ?? GetDefaultErrorMessage(statusCode);
        }

        private string GetDefaultErrorMessage(int statusCode)
        => statusCode switch
        {
            400 => "A badRequest",
            401 => "Authorized you are not",
            404 => " Resource not found",
            500 => "Errors",
            _=> null
        };
    }
}
