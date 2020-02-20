using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using ProspaAspNetCoreApi.Application.HealthChecks;

namespace ProspaAspNetCoreApi
{
    public static class StartupHealth
    {
        public static IApplicationBuilder UseDefaultHealth(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return builder;
        }

        public static IServiceCollection AddDefaultHealth(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("sample_health_check");

            services.AddHealthChecksUI();

            return services;
        }
    }
}
