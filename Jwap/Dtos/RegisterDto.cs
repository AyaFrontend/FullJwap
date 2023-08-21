using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        public string fName { set; get; }

        [Required]
        [MinLength(3)]
        public string lName { set; get; }
        public string profilePicture { set; get; } 
        public string caption { set; get; }

        [Required]
        [EmailAddress]
        public string email { set; get; }

        [Required]
      //  [RegularExpression(pattern:"[a-zA-Z0-9]{6,0}")]
        public string password { set; get; }
    }
}
