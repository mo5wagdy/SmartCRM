using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCRM.Domain.Entities;


namespace SmartCRM.Infrastructure.Data.Configurations
{
    public class InteractionConfiguration : IEntityTypeConfiguration<Interaction>
    {
        public void Configure(EntityTypeBuilder<Interaction> builder)
        {
            // Constraints

            builder.HasKey(a => a.InteractionId);

            builder.Property(a => a.Title).IsRequired().HasMaxLength(250);
            builder.Property(a => a.Description).HasMaxLength(1000);
            builder.Property(a => a.ImteractionType).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Status).IsRequired().HasMaxLength(50);
            builder.Property(a => a.InteractionDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(a => a.UpdatedAt).IsRequired(false);
            builder.Property(a => a.IsActive).HasDefaultValue(true);
            builder.Property(a => a.IsDeleted).HasDefaultValue(false);

            // Relationships

            builder.HasOne(a => a.User)
                   .WithMany(u => u.Interactions)
                   .HasForeignKey(a => a.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Customer)
                   .WithMany(c => c.Interactions)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Deal)
                   .WithMany(d => d.Interactions)
                   .HasForeignKey(a => a.DealId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
