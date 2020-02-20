using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class StartupMvc
    {
        public static IServiceCollection AddDefaultContextAccessors(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
