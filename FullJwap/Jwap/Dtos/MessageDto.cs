using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Dtos
{
    public class MessageDto
    {
        public string Id { set; get; }
        public string SenderId { set; get; }

        public string ConnectionId { set; get; }
        public string RecieverId { set; get; }
      
        public DateTime SendDate { set; get; }
        public bool IsRead { set; get; } = false;
        public bool IsDelete { set; get; } = false;
        public string Messege { set; get; }
        public string MessegeType { set; get; } 
        public string AudioUrl { set; get; } = "";

    }
}
