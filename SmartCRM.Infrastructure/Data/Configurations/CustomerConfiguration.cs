using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            //Constraints

            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.Address)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(c => c.Status)
                   .HasMaxLength(50)
                   .HasDefaultValue("Active");

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //Relationships

            builder.HasMany(c => c.Deals)
                   .WithOne(d => d.Customer)
                   .HasForeignKey(d => d.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Notes)
                   .WithOne(n => n.Customer)
                   .HasForeignKey(n => n.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Tickets)
                   .WithOne(t => t.Customer)
                   .HasForeignKey(t => t.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Leads)
                   .WithOne(l => l.Customer)
                   .HasForeignKey(l => l.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Interactions)
                   .WithOne(a => a.Customer)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
