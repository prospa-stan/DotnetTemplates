using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProspaWorker.Workers;

namespace ProspaWorker
{
    public static class StartupWorkers
    {
        public static IServiceCollection SetupWorkers(this IServiceCollection services, HostBuilderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            services.AddHostedService<Worker>();

            return services;
        }
    }
}
