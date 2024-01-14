using FluentValidation.AspNetCore;
using ShopFront.Domain.Repositories;
using ShopFront.Infrastructure.Repositories;
using FluentValidation;
using System.Reflection;

namespace ShopFront.WebApi.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
