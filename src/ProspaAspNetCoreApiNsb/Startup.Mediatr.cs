using MediatR;
using ProspaAspNetCoreApiNsb;
using ProspaAspNetCoreApiNsb.Infrastructure.Behaviours;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class StartupMessaging
    {
        public static IServiceCollection AddDefaultMediatr(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Program));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }
    }
}
