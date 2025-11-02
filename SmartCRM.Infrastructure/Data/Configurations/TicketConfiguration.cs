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
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            //Constraints

            builder.HasKey(t => t.TicketID);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Open");

            builder.Property(t => t.Priority)
                   .HasMaxLength(50)
                   .HasDefaultValue("Medium");

            builder.Property(t => t.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //Relationships

            builder.HasOne(t => t.Customer)
                   .WithMany(c => c.Tickets)
                   .HasForeignKey(t => t.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.User)
                   .WithMany(u => u.Tickets)
                   .HasForeignKey(t => t.AssignedTo)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
