using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class UserDto
    {
        public string Id { set; get; }
        public string Email { set; get; }
        public string DisplayName { set; get; }
        public string Token { set; get; }
        public string ProfilePicture { set; get; } = "Avatar";
    }
}
