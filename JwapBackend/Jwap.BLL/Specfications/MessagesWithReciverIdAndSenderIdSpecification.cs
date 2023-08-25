using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.BLL.Specfications
{
    public class MessagesWithReciverIdAndSenderIdSpecification : BaseSpecfication<Message>
    {
        public MessagesWithReciverIdAndSenderIdSpecification(string ReciverId , string SenderId) :
            base(x=> (x.RecieverId == ReciverId || x.RecieverId == SenderId) && (x.SenderId == SenderId || x.SenderId == ReciverId) && !x.IsDelete)
        {
             ApplyInclude(x => x.Reciever);
             ApplyInclude(x => x.Sender);
            AddOrderBy(x => x.SendDate);
        }
    }
}
