using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DependencyInjection;

namespace AuthenticationApi.Infrastructure.DependencyInjection {
    public static class ServiceContainer {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config) {
            // Add database connectivity
            // JWT Add Authentication Scheme
            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            services.AddScoped<IUser, UserRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app) {
            // Register middleware sucs as:
            // Global Exception : Handle external errors.
            // Listen Only To Api Gateway : block all outsiders call.
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}