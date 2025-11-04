
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using SmartCRM.Domain.Entities;
//using SmartCRM.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;

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

            // Seed Admin User
            if (!await ctx.Users.AnyAsync())
            {
                ctx.Users.Add(new User
                {
                    FullName = "Administrator",
                    Email = "admin@local",
                    Phone = "000-000-0000",
                    PasswordHash = /*PasswordHasher.Hash("Admin@123!")*/ "1234566",
                    Role = "Admin",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false,
                    ImagePath = null
                });
                await ctx.SaveChangesAsync();
            }

            var admin = await ctx.Users.FirstOrDefaultAsync(u => u.Email == "admin@local");

            // Seed Product
            if (!await ctx.Products.AnyAsync())
            {
                ctx.Products.Add(new Product
                {
                    Name = "Sample Product",
                    Description = "Seeded product for demo",
                    Price = 99.99m,
                    QuantityInStock = 100,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false,
                    ImagePath = null
                });
                await ctx.SaveChangesAsync();
            }

            var product = await ctx.Products.FirstOrDefaultAsync();

            // Seed Customer
            if (!await ctx.Customers.AnyAsync())
            {
                ctx.Customers.Add(new Customer
                {
                    FullName = "Default Customer",
                    Email = "customer@local",
                    Phone = "111-222-3333",
                    Address = "123 Main St",
                    CompanyName = "Default Co",
                    CustomerType = "Prospect",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false,
                    Status = "Active"
                });
                await ctx.SaveChangesAsync();
            }

            var customer = await ctx.Customers.FirstOrDefaultAsync();

            // Seed Lead
            if (!await ctx.Leads.AnyAsync())
            {
                ctx.Leads.Add(new Lead
                {
                    ContactName = "Jane Lead",
                    Email = "lead@local",
                    ContactPhone = "222-333-4444",
                    Source = "Web",
                    Status = "New",
                    Notes = "Interested in product.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false,
                    AssignedTo = admin?.UserId,
                    User = admin,
                    CustomerId = customer?.CustomerId,
                    Customer = customer
                });
                await ctx.SaveChangesAsync();
            }

            // Seed Deal
            if (!await ctx.Deals.AnyAsync())
            {
                ctx.Deals.Add(new Deal
                {
                    Title = "Initial Deal",
                    Value = 5000,
                    Stage = "Negotiating",
                    ExpectedCloseDate = DateTime.UtcNow.AddMonths(1),
                    CreatedDate = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false,
                    CustomerId = customer!.CustomerId,
                    Customer = customer,
                    AssignedTo = admin?.UserId,
                    User = admin
                });
                await ctx.SaveChangesAsync();
            }

            var deal = await ctx.Deals.FirstOrDefaultAsync();

            // Seed DealProduct
            if (!await ctx.DealProducts.AnyAsync() && deal != null && product != null)
            {
                ctx.DealProducts.Add(new DealProduct
                {
                    DealId = deal.DealId,
                    ProductId = product.ProductId,
                    Quantity = 2,
                    UnitPrice = product.Price
                });
                await ctx.SaveChangesAsync();
            }

            // Seed Interaction
            if (!await ctx.Interactions.AnyAsync())
            {
                ctx.Interactions.Add(new Interaction
                {
                    Title = "Intro Call",
                    Description = "Discussed requirements.",
                    InteractionDate = DateTime.UtcNow,
                    ImteractionType = "Call",
                    Status = "Completed",
                    RelatedTo = "Lead",
                    RelatedId = 1,
                    CreatedBy = admin!.UserId,
                    AssignedTo = admin.UserId,
                    CustomerId = customer!.CustomerId,
                    DealId = deal?.DealId,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false
                });
                await ctx.SaveChangesAsync();
            }

            // Seed Note
            if (!await ctx.Notes.AnyAsync())
            {
                ctx.Notes.Add(new Note
                {
                    Content = "First note for customer.",
                    RelatedTo = "Customer",
                    RelatedId = customer!.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false,
                    CustomerId = customer.CustomerId,
                    DealId = deal?.DealId,
                    UserId = admin!.UserId,
                    User = admin
                });
                await ctx.SaveChangesAsync();
            }

            // Seed Ticket
            if (!await ctx.Tickets.AnyAsync())
            {
                ctx.Tickets.Add(new Ticket
                {
                    Title = "Support Request",
                    Description = "Customer needs help with onboarding.",
                    Status = "Open",
                    Priority = "Medium",
                    CreatedAt = DateTime.UtcNow,
                    CustomerId = customer!.CustomerId,
                    AssignedTo = admin!.UserId,
                    User = admin
                });
                await ctx.SaveChangesAsync();
            }

            // Seed Company
            if (!await ctx.Companies.AnyAsync())
            {
                ctx.Companies.Add(new Company
                {
                    CompanyName = "Default Co",
                    Address = "123 Main St",
                    Industry = "Software",
                    Email = "info@defaultco.com",
                    Phone = "555-555-5555",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedAt = null,
                    IsActive = true,
                    IsDeleted = false
                });
                await ctx.SaveChangesAsync();
            }
        }
    }
}