using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jwap.DAL.Entities
{
    public class Connections  : BaseEntity
    {
       
        
        public string UserId { set; get; }
        public virtual User User { set; get; }

        
        public string UserConnectionId { set; get; }
        public bool Online { set; get; } = true;
    }
}
