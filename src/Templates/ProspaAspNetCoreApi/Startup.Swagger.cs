using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Prospa.Extensions.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
// ReSharper restore CheckNamespace
{
    public static class StartupSwagger
    {
        public static IServiceCollection AddDefaultSwagger(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            services.AddSwaggerGen(
                options =>
                {
                    var assembly = typeof(StartupSwagger).GetTypeInfo().Assembly;
                    var assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                    var apiVersionDescriptionProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();

                    AddDefaultOptions(options, assembly);
                    AddDefaultOperationFilters(provider, options);
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                });

            return services;
        }

        public static IApplicationBuilder UseDefaultSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(
                options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                    });
                });

            return app;
        }

        public static IApplicationBuilder UseDefaultSwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(
                options =>
                {
                    var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                    foreach (var version in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{version.GroupName}/swagger.json", $"My API {version.GroupName}");
                    }

                    options.RoutePrefix = string.Empty;
                    // options.SwaggerVersionedJsonEndpoints(provider);
                });

            return app;
        }

        private static void AddDefaultOperationFilters(IServiceProvider provider, SwaggerGenOptions options)
        {
            var authzOptions = provider.GetRequiredService<AuthOptions>();
        }

        private static void AddDefaultOptions(SwaggerGenOptions options, Assembly assembly)
        {
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.DescribeAllParametersInCamelCase();
        }
    }
}