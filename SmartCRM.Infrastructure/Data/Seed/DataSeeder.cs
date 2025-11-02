using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using SmartCRM.Domain.Entities;

namespace SmartCRM.Infrastructure.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<CrmDbContext>();

            // ensure migrations applied
            await ctx.Database.MigrateAsync();

            if (!await ctx.Users.AnyAsync())
            {
                ctx.Users.Add(new User
                {
                    FullName = "Administrator",
                    Email = "admin@local",
                    Phone = "",
                    PasswordHash = "replace-with-secure-hash",
                    Role = "Admin",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (!await ctx.Products.AnyAsync())
            {
                ctx.Products.Add(new Product
                {
                    Name = "Sample Product",
                    Description = "Seeded product",
                    Price = 0m,
                    QuantityInStock = 100,
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (!await ctx.Customers.AnyAsync())
            {
                ctx.Customers.Add(new Customer
                {
                    FullName = "Default Customer",
                    Email = "customer@local",
                    Phone = "",
                    Address = "",
                    CustomerType = "Prospect",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                });
            }

            await ctx.SaveChangesAsync();
        }
    }
}