using App.Metrics;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;
using Microsoft.AspNetCore.Hosting;

namespace ProspaAspNetCoreApi
{
    public static class ProgramHealth
    {
        public static IWebHostBuilder UseDefaultHealth(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.UseHealth();

            return webHostBuilder;
        }

        public static IWebHostBuilder ConfigureDefaultHealth(this IWebHostBuilder webHostBuilder, IMetrics metrics)
        {
            webHostBuilder.ConfigureHealthWithDefaults((context, builder) =>
            {
               builder.Report.ToMetrics(metrics);
            });

            return webHostBuilder;
        }
    }
}