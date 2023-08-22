using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public class FriendsWithSpecification : BaseSpecfication<Friends>
    {
        public FriendsWithSpecification(string reciverId):base(
            x=> x.FreindId == reciverId
            )
        {

        }
    }
}
