﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prospa.Extensions.AspNetCore.Mvc.Core.StartupFilters;
using Serilog;

namespace ProspaAspNetCoreApi
{
    public partial class Program
    {
        public static int Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly, check the application's WebHost configuration.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddDefaultSources(args);
                })
                .UseSerilog(ConfigureLogger)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder
                        .ConfigureKestrel(options => { options.AddServerHeader = false; })
                        .ConfigureServices((context, services) =>
                        {
                            services.AddSingleton<IStartupFilter>(
                                new RequireEndpointKeyStartupFilter(
                                    new[] { "/health", "/metrics", "/metrics-text", "/env", "/docs" },
                                context.Configuration.GetValue<string>(Constants.Auth.EndpointKey)));
                        })
                        .UseStartup<Startup>();
                });
    }
}
