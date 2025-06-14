using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using SharedLibrary.Logs;

namespace OrderApi.Application.DependencyInjection {
    public static class ServiceContainer {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config) {
            // Register HhtpClient service
            // Create dependency injection
            services.AddHttpClient<IOrderService, OrderService>(options => {
                options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(1);
            });

            // Create retry strategy
            var retryStrategy = new RetryStrategyOptions() {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args => {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }
            };

            // Use retry strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder => {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}