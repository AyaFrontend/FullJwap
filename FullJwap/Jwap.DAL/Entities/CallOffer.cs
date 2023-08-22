using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Entities
{
    public class CallOffer : BaseEntity
    {
        public string CallerId { set; get; }
        public virtual User Caller { set; get; }

        public string CalleeId { set; get; }
        public virtual User Callee { set; get; }

        public DateTime CallDate { set; get; } = DateTime.Now;

        public bool CallState { set; get; }
    }
}
