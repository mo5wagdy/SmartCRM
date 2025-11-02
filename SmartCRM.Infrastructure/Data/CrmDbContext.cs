using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCRM.Domain.Entities;
using SmartCRM.Infrastructure.Data.Configurations;
using Microsoft.IdentityModel.Abstractions;

namespace SmartCRM.Infrastructure.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Deal> Deals => Set<Deal>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Interaction> Interactions => Set<Interaction>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<DealProduct> DealProducts => Set<DealProduct>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrmDbContext).Assembly);
        }
    }
}
