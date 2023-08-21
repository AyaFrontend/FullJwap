using Jwap.BLL.Interfaces;
using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<T> Repository<T>() where T : class ;
        public Task<int> Complete();
    }
}
