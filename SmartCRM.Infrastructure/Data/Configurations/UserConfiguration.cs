using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCRM.Domain.Entities;

namespace SmartCRM.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Constraints

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Phone).HasMaxLength(25);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Active");
            builder.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(u => u.UpdatedAt).IsRequired(false);
            builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(u => u.ImagePath).HasMaxLength(500).IsRequired(false);

            //Relationships

            builder.HasMany(u => u.Leads)
                   .WithOne(l => l.User)
                   .HasForeignKey(l => l.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Deals)
                   .WithOne(d => d.User)
                   .HasForeignKey(d => d.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Tickets)
                   .WithOne(t => t.User)
                   .HasForeignKey("AssignedTo")
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Interactions)
                   .WithOne(i => i.User)
                   .HasForeignKey(i => i.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
