using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<string> Errors { set; get; }

        public ApiValidationErrorResponse() : base(400 , "Validation error, please enter coorect data")
        {
            
        }
    }
}
