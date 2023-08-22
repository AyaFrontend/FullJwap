using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class LoginDto
    {
        

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }
        public bool RememberMe { set; get; } = false;

    }
}
