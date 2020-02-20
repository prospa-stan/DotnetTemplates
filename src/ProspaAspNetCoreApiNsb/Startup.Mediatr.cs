using MediatR;
using ProspaAspNetCoreApiNsb;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class StartupMessaging
    {
        public static IServiceCollection AddDefaultMediatr(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Program));

            return services;
        }
    }
}
