using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user, UserManager<User> userManager);
    }
}
