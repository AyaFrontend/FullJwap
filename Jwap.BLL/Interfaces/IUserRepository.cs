using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetFriends(string userId);
      
    }
}
                                                        


