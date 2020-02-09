using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ProspaAspNetCoreApi;
using ProspaAspNetCoreApi.ConfigureOptions;

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

        public static IMvcCoreBuilder AddDefaultMvcOptions(this IMvcCoreBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, FormattersMvcOptionsSetup>();

            return builder;
        }
    }
}