using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class ForgetPasswordDto
    {
        [Required]
        public string Email { set; get; }

       
    }
}
