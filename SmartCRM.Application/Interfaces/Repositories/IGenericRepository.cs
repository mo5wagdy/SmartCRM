using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> Query();
        IQueryable<T> QueryNoTracking();
        Task<List<T>> ToListAsync(IQueryable<T> query);
        Task<int> CountAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
        Task<decimal?> SumAsync(IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default);
        Task<DateTime?> MaxAsync(IQueryable<T> query, Expression<Func<T, DateTime>> selector, CancellationToken cancellationToken = default);
    }
}
