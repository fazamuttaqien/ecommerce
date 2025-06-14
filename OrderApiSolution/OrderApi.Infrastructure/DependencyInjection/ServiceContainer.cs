using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;
using SharedLibrary.DependencyInjection;

namespace OrderApi.Infrastructure.DependencyInjection {
    public static class ServiceContainer {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config) {
            // Add database connectivity
            // Add authentication scheme
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:Filename"]!);

            // Create dependency injection
            services.AddScoped<IOrder, OrderRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app) {
            // Register middleware sucs as:
            // Global Exception -> handle external errors
            // ListenToApiGateway Only -> block all outsiders calls
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}