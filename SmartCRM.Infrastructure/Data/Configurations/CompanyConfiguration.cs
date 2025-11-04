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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.CompanyName).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Address).IsRequired().HasMaxLength(300);
            builder.Property(c => c.Industry).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Phone).IsRequired().HasMaxLength(25);
            builder.Property(c => c.CreatedDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(c => c.UpdatedAt).IsRequired(false);
            builder.Property(c => c.IsActive).HasDefaultValue(true);
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}
