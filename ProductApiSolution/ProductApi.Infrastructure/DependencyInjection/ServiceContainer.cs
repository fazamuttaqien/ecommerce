using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using SharedLibrary.DependencyInjection;

namespace ProductApi.Infrastructure.DependencyInjection {
    public static class ServiceContainer {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config) {
            // Add database connectivity
            // Add authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FineName"]!);

            // Create Dependency Injection (DI)
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app) {
            // Register middleware sucs as:
            // Global Exception: handles external errors.
            // Listen to Only Api Gateway: blocks all outsider calls;
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}