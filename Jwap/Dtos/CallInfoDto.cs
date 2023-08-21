using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class CallInfoDto
    {
        public string type { set; get; }
        public long size { set; get; }
    }
}
