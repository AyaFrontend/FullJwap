using Jwap.BLL.Services;
using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public class ConnectionSpecifications : BaseSpecfication<Connections>
    {
        public ConnectionSpecifications(string connectionId ) : base(
            c => c.UserConnectionId == connectionId )
        { }

        public ConnectionSpecifications(string userId , string recieverId) : base(
            c => c.UserId == userId || c.UserId == recieverId)
        { }
       
    }
}