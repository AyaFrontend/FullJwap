using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Entities
{
    public class Message : BaseEntity
    {
        public string SenderId { set; get; }
       public virtual User Sender { set; get; }

        public string RecieverId { set; get; }
       public virtual User Reciever { set; get; }

        public DateTime SendDate { set; get; } = DateTime.Now;
        public bool IsRead { set; get; } = false;
        public bool IsDelete { set; get; } = false;
        public string Messege { set; get; }
        public string MessegeType { set; get; }
        public string AudioUrl { set; get; }


    }
}
