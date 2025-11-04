using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCRM.Domain.Entities;

namespace SmartCRM.Infrastructure.Data.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(n => n.NoteId);

            builder.Property(n => n.Content).IsRequired();
            builder.Property(n => n.RelatedTo).IsRequired();
            builder.Property(n => n.RelatedId).IsRequired();
            builder.Property(n => n.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(n => n.UpdatedAt).IsRequired(false);
            builder.Property(n => n.IsActive).HasDefaultValue(true);
            builder.Property(n => n.IsDeleted).HasDefaultValue(true);

            // Relationships
            builder.HasOne(n => n.Customer)
                   .WithMany(c => c.Notes)
                   .HasForeignKey(n => n.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Deal)
                   .WithMany(d => d.Notes)
                   .HasForeignKey(n => n.DealId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notes)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}