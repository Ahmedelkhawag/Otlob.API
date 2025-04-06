using Otlob.Core.Models;
using Otlob.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specification
        Task</*IEnumerable*/ IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specification
        Task</*IEnumerable*/ IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
        Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification);
        #endregion
    }
}
