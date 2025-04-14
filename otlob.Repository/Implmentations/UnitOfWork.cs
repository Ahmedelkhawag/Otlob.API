using Otlob.Core.Interfaces;
using Otlob.Core.Models;
using Otlob.Core.Repositories;
using Otlob.Repository.Data;
using Otlob.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository.Implmentations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly API1DbContext _context;
        private readonly Hashtable _repositories;

        public UnitOfWork(API1DbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public async Task<int> CompleteAsync()
        {
         return await  _context.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
          return  _context.DisposeAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;
            if (!(_repositories.ContainsKey(type)))
            { 
            var repository = new GenericRepository<T>(_context);
                _repositories.Add(type, repository);
            }
            return (IGenericRepository<T>)_repositories[type];
        }
    }
}
