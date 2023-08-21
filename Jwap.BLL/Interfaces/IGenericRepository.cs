using Jwap.BLL.Specfications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        public Task DeleteAsync(T entity);
        public Task AddAsync(T entity);
        public void AddSync(T entity);
        public Task UpdateAsync(T entity);
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(string id);
        public Task<IEnumerable<T>> Search(ISpecfication<T> spec);
    }
}
