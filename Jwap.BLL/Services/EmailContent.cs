using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.BLL.Services
{
    public class EmailContent
    {
        public string From { set; get; } = "ayamohamedbdelrahman868@gmail.com";
        public string To { set; get; }
        public string Body { set; get; }
        public string Title { set; get; }
    }
}
