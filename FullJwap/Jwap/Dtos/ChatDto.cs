using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class ChatDto
    {
        public string SenderId { set; get; }
        public string ReceiverId { set; get; }

        public ICollection<MessageDto> Messages { set; get; } = new HashSet<MessageDto>();
    }
}
