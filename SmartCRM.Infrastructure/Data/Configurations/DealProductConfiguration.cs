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
    public class DealProductConfiguration : IEntityTypeConfiguration<DealProduct>
    {
        public void Configure(EntityTypeBuilder<DealProduct> builder)
        {
            //Constraints

            builder.HasKey(dp => new { dp.DealId, dp.ProductId});

            builder.Property(dp => dp.Quantity)
                   .IsRequired()
                   .HasDefaultValue(1);


            builder.Property(dp => dp.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            //Relationships

            builder.HasOne(dp => dp.Deal)
                   .WithMany(d => d.DealProducts)
                   .HasForeignKey(dp => dp.DealId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dp => dp.Product)
                   .WithMany(p => p.DealProducts)
                   .HasForeignKey(dp => dp.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
