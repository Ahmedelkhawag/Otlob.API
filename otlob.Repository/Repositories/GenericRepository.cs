using Microsoft.EntityFrameworkCore;
using Otlob.Core.Models;
using Otlob.Core.Repositories;
using Otlob.Core.Specifications;
using Otlob.Repository.Data;
using Otlob.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly API1DbContext _dbContext;

        public GenericRepository(API1DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Without Specification
        public async Task</*IEnumerable*/ IReadOnlyList<T>> GetAllAsync()
               => await _dbContext.Set<T>().ToListAsync();


        public async Task<T> GetByIdAsync(int id)
                => await _dbContext.Set<T>().FindAsync(id);
        #endregion


        #region With Specification
        public async Task</*IEnumerable*/ IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
               => await ApplySpecification(specification).ToListAsync();


        public async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification)
               => await ApplySpecification(specification).FirstOrDefaultAsync();


        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
               => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);
        #endregion
    }
}
