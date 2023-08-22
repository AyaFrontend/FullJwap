using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class ConnectionsDto
    {
        public string Id { set; get; }
        public string FName { set; get; }
        public string LName { set; get; }
        public string ProfilePicture { set; get; } = "Avatar";
        public bool Online { set; get; }
        public string LastMessage { set; get; }


    }
}
