using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Errors
{
    public class ApiExceptionErrorResponse : ApiResponse
    {
        string Details { set; get; }

        public ApiExceptionErrorResponse(int statusCode , string message = null, string details = null)
                                        :base(statusCode , message)
        {
            Details = details;
        }


    }
}
