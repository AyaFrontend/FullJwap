using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public class UsersWithSpecification : BaseSpecfication<User> 
    {
        public UsersWithSpecification(string  username): base(x => x.UserName.ToLower().Contains(username.ToLower()) )
        {

        }
    }
}
