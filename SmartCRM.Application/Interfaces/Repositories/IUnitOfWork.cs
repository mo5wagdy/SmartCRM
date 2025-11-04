using SmartCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Customer> Customers { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Lead> Leads { get; }
        IGenericRepository<Deal> Deals { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Note> Notes { get; }
        IGenericRepository<Interaction> Interactions { get; }
        IGenericRepository<Ticket> Tickets { get; }
        IGenericRepository<Company> Companies { get; }
        IGenericRepository<DealProduct> DealProducts { get; }
        Task<int> SaveAsync();
    }
}
