using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class TrackDto
    {
        public IFormFile File { set; get; }
        public string Data { set; get; }
    }
}
