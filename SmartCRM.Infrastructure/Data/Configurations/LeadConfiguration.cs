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
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            //Constraints

            builder.HasKey(l => l.LeadId);

            builder.Property(l => l.ContactName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(l => l.ContactPhone)
                   .IsRequired()
                   .HasMaxLength(25);

            builder.Property(l => l.Source)
                   .HasMaxLength(100);

            builder.Property(l => l.Status)
                   .HasMaxLength(50);

            builder.Property(l => l.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //Relationships

            builder.HasOne(l => l.User)
                   .WithMany(u => u.Leads)
                   .HasForeignKey(l => l.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(l => l.Customer)
                   .WithMany(c => c.Leads)
                   .HasForeignKey(l => l.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
