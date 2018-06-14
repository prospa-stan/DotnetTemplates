using App.Metrics;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProspaAspNetCoreApi.StartupFilters;

namespace ProspaAspNetCoreApi
{
    public static class ProgramMetrics
    {
        public static IWebHostBuilder UseDefaultMetrics(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureServices((context, services) => services.AddSingleton<IStartupFilter>(new RequireKeyForMetricsAndHealthStartupFilter()));

            webHostBuilder.UseMetrics();

            if (!Constants.Environments.IsDevelopment())
            {
                webHostBuilder.UseApplicationInsights();
            }

            return webHostBuilder;
        }

        public static IWebHostBuilder ConfigureDefaultMetrics(this IWebHostBuilder webHostBuilder)
        {
            // Samples with weight of less than 10% of average should be discarded when rescaling
            const double minimumSampleWeight = 0.001;

            webHostBuilder.ConfigureMetricsWithDefaults(builder =>
            {
                builder.SampleWith.ForwardDecaying(
                    AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                    AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                    minimumSampleWeight: minimumSampleWeight);
            });

            return webHostBuilder;
        }
    }
}