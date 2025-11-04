using Microsoft.EntityFrameworkCore;
using SmartCRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


    }
}
