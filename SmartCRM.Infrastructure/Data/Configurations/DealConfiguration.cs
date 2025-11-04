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
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            //Constraints

            builder.HasKey(d => d.DealId);

            builder.Property(d => d.Title).IsRequired().HasMaxLength(200);
            builder.Property(d => d.Value).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(d => d.Stage).IsRequired().HasMaxLength(100);
            builder.Property(d => d.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(d => d.UpdatedAt).IsRequired(false);
            builder.Property(d => d.IsActive).HasDefaultValue(true);
            builder.Property(d => d.IsDeleted).HasDefaultValue(false);

            //Relationships

            builder.HasOne(d => d.Customer)
                   .WithMany(c => c.Deals)
                   .HasForeignKey(d => d.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.User)
                   .WithMany(u => u.Deals)
                   .HasForeignKey(d => d.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(d => d.DealProducts)
                   .WithOne(dp => dp.Deal)
                   .HasForeignKey(dp => dp.DealId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Notes)
                   .WithOne(n => n.Deal)
                   .HasForeignKey(n => n.DealId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Interactions)
                   .WithOne(a => a.Deal)
                   .HasForeignKey(a => a.DealId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
