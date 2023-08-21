using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Entities
{
    public class Friends 
    {
        public virtual User User { set; get; }
        public string UserId { set; get; }

        
        public string FreindId { set; get; }
       
    }
}
