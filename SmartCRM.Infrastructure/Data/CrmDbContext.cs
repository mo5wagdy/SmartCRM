using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCRM.Domain.Entities;

namespace SmartCRM.Infrastructure.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Deal> Deals => Set<Deal>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints if needed
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Deals)
                .WithOne(d => d.Customer)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Notes)
                .WithOne(n => n.Customer)
                .HasForeignKey(n => n.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
