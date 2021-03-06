﻿using System.Text.Json.Serialization;
using CorrelationId;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prospa.Extensions.ApplicationInsights;
using ProspaAspNetCoreApi.ConfigureOptions;

namespace ProspaAspNetCoreApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseRequireHttps("/ping", "/api/ping")
                .UseDefaultHealth()
                .UseCorrelationId(new CorrelationIdOptions { UpdateTraceIdentifier = false })
                .UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = Constants.HttpHeaders.ForwardedHeaders })
                .UseDefaultSecurityHeaders(_hostingEnvironment)
                .UseAuthentication()
                .UseDefaultDiagnostics(_hostingEnvironment)
                .UseCors(Constants.Cors.AllowAny)
                .UseDefaultSwagger()
                .UseDefaultSwaggerUi()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                   endpoints.MapControllers();
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddCoreServices(services);
            AddApplicationServices(services);
        }

        private void AddApplicationServices(IServiceCollection services)
        {
            // TODO: Add app specific services
        }

        private void AddCoreServices(IServiceCollection services)
        {
            services.AddCorrelationId();

            services.AddDefaultHealth();

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddDefaultValidation();

            services.AddSingleton<IConfigureOptions<CorsOptions>, CorsOptionsSetup>();
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = Constants.Versioning.GroupNameFormat);

            services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddDefaultAuthenticationAndAuthorization(_configuration)
                .AddApiVersioning()
                .AddDefaultContextAccessors()
                .AddDefaultDiagnostics(_configuration)
                .AddDefaultSwagger();
        }
    }
}
