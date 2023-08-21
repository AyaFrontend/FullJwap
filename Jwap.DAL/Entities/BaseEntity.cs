using System;
using System.Collections.Generic;
using System.Text;

namespace Jwap.DAL.Entities
{
    public class BaseEntity
    {
        public string Id { set; get; } = Guid.NewGuid().ToString();
    }
}
