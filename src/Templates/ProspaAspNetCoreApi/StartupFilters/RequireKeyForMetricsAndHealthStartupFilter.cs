using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ProspaAspNetCoreApi.StartupFilters
{
    public class RequireKeyForMetricsAndHealthStartupFilter : IStartupFilter
    {
        private static readonly string[] Endpoints = { "/health", "/metrics", "/metrics-text", "/env" };
        private readonly IConfiguration _configuration;

        public RequireKeyForMetricsAndHealthStartupFilter(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return RequireSecretToMetricsAndHealth;

            void RequireSecretToMetricsAndHealth(IApplicationBuilder app)
            {
                var token = _configuration.GetValue<string>("MetricsHealthEndpointToken");

                app.Use(async (context, next2) =>
                {
                    var key = ExtractToken(context);

                    if (Endpoints.Any(e => context.Request.Path.Value == e))
                    {
                        if (key != token)
                        {
                            context.Abort();
                            return;
                        }
                    }

                    await next2.Invoke();
                });

                next(app);
            }
        }

        private static string ExtractToken(HttpContext context)
        {
            return context.Request.QueryString.HasValue && context.Request.Query.ContainsKey(nameof(Constants.EndpointKey))
                ? context.Request.Query[nameof(Constants.EndpointKey)]
                : StringValues.Empty;
        }
    }
}
