using Microsoft.Extensions.DependencyInjection;
using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Application.Interfaces.Services;
using SmartCRM.Application.Mapping;
using SmartCRM.Application.Services;
using SmartCRM.Infrastructure.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories / UoW only
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper profiles located in Application project
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());

            // FluentValidation validators from Application assembly (if validators exist)
            services.AddValidatorsFromAssembly(typeof(AutoMapperProfiles).Assembly);

            // Register application services (business logic)
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILeadService, LeadService>();

            // TODO: register other services as they are implemented:
            // services.AddScoped<IDealService, DealService>();
            // services.AddScoped<IProductService, ProductService>();
            // services.AddScoped<INoteService, NoteService>();
            // services.AddScoped<IActivityService, ActivityService>();
            // services.AddScoped<IUserService, UserService>();

            return services;
        }

    }
}
