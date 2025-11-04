using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Domain.Entities;
using SmartCRM.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CrmDbContext _db;

        public UnitOfWork(CrmDbContext db)
        {
            _db = db;
            Customers = new GenericRepository<Customer>(_db);
            Users = new GenericRepository<User>(_db);
            Leads = new GenericRepository<Lead>(_db);
            Deals = new GenericRepository<Deal>(_db);
            Products = new GenericRepository<Product>(_db);
            Notes = new GenericRepository<Note>(_db);
            Interactions = new GenericRepository<Interaction>(_db);
            Tickets = new GenericRepository<Ticket>(_db);
            Companies = new GenericRepository<Company>(_db);
            DealProducts = new GenericRepository<DealProduct>(_db);
        }

        public IGenericRepository<Customer> Customers { get; }
        public IGenericRepository<User> Users { get; }
        public IGenericRepository<Lead> Leads { get; }
        public IGenericRepository<Deal> Deals { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Note> Notes { get; }
        public IGenericRepository<Interaction> Interactions { get; }
        public IGenericRepository<Ticket> Tickets { get; }
        public IGenericRepository<Company> Companies { get; }
        public IGenericRepository<DealProduct> DealProducts { get; }

        public async Task<int> SaveAsync() => await _db.SaveChangesAsync();
    }
}
