using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Interfaces
{
    public interface IMessageRepository
    {
        public string GetLastMessageById(string Id);
    }
}
