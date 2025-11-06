using Microsoft.EntityFrameworkCore;
using SmartCRM.Infrastructure.Data;
using SmartCRM.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SmartCRM.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CrmDbContext _db;
        protected readonly DbSet<T> _Set;

        public GenericRepository(CrmDbContext db)
        {
            _db = db;
            _Set = _db.Set<T>();
        }

        public async Task<T?> GetByIdAsync(object id) => await _Set.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _Set.AsNoTracking().ToListAsync();
        public async Task AddAsync(T entity)
        {
            await _Set.AddAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _Set.Update(entity);
        }
        public async Task DeleteAsync(T entity)
        {
            _Set.Remove(entity);
        }
        public IQueryable<T> Query() => _Set.AsQueryable();
        public IQueryable<T> QueryNoTracking() => _Set.AsNoTracking();

        public async Task<List<T>> ToListAsync(IQueryable<T> query) => await query.ToListAsync();

        public async Task<int> CountAsync(IQueryable<T> query, CancellationToken cancellationToken = default) => await query.CountAsync(cancellationToken);

        public async Task<decimal?> SumAsync(IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default) => await  query.SumAsync(selector, cancellationToken);

        public async Task<DateTime?> MaxAsync(IQueryable<T> query, Expression<Func<T, DateTime>> selector, CancellationToken cancellationToken = default) => await query.MaxAsync(selector, cancellationToken);

    }
}
