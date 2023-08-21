using Jwap.BLL.Interfaces;
using Jwap.DAL.Data;
using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Repository
{
    public class UserRepository : IUserRepository
    {
        private DataContext _dbContext;
        public UserRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<User> GetFriends(string userId)
        {
            var friends = from u in _dbContext.AppUsers
                          from f in _dbContext.Friends
                          where (u.Id == f.UserId )
                          from r in _dbContext.AppUsers
                          where(r.Id == f.FreindId)
                        
                          select r;

            return  friends.AsEnumerable();
        }

       
    }
}
