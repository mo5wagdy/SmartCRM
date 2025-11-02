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

            builder.Property(n => n.Content)
                .IsRequired();

            builder.Property(n => n.RelatedTo)
                .IsRequired();

            builder.Property(n => n.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Relationships
            builder.HasOne(n => n.Customer)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade here

            builder.HasOne(n => n.Deal)
                .WithMany(d => d.Notes)
                .HasForeignKey(n => n.DealId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Or Restrict, as needed
        }
    }
}