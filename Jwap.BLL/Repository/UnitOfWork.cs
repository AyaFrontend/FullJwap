using Jwap.BLL.Interfaces;
using Jwap.BLL.Repository;
using Jwap.DAL.Data;
using Jwap.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jwap.BLL.Repository
{
    public class UnitOfWork : IUnitOfWork 
    {
        private Hashtable repositories;
        private readonly DataContext _context;

        public UnitOfWork (DataContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            
            if (repositories == null) repositories = new Hashtable();
            var key = typeof(T).Name;
            if (!repositories.ContainsKey(key))
            {
                var repo = new GenericRepository<T>(_context);
                repositories.Add(key, repo);
            }

            return (IGenericRepository<T>)repositories[key];
        }

       

        public async Task<int> Complete()
        => await _context.SaveChangesAsync();

        public void Dispose()
        {
            
        }
    }
}
