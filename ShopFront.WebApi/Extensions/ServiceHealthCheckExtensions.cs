using ShopFront.Infrastructure;

namespace ShopFront.WebApi.Extensions
{
    public static class ServiceHealthCheckExtensions
    {
        public static IServiceCollection RegisterCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddDbContextCheck<ApiDbContext>();

            return services;
        }
    }
}
