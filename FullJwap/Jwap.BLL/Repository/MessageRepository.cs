using Jwap.BLL.Interfaces;
using Jwap.DAL.Data;
using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        public MessageRepository(DataContext context)
        {
            _context = context;
        } 

        public  string GetLastMessageById(string Id)
        {
          string message =
                _context.Set<Message>().Where(m => (m.RecieverId == Id  || m.SenderId == Id) && (m.Messege !="" || m.Messege != null) && !m.IsDelete).OrderByDescending(o => o.SendDate).Select(m=> m.Messege).FirstOrDefault();
            return message;
        }
    }
}
