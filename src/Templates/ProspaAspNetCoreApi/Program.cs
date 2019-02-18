using System;
using System.IO;
using App.Metrics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace ProspaAspNetCoreApi
{
    public sealed class Program
    {
        public static int Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            try
            {
                webHost.Run();

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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var metrics = AppMetrics.CreateDefaultBuilder().BuildDefaultMetrics();

            return WebHost.CreateDefaultBuilder(args)
                   .UseKestrel(options => options.AddServerHeader = false)
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureDefaultAppConfiguration(args)
                   .ConfigureDefaultMetrics(metrics)
                   .ConfigureDefaultHealth(metrics)
                   .UseSerilog(ProgramLogger.ConfigureLogger)
                   .UseDefaultMetrics()
                   .UseDefaultHealth()
                   .UseStartup<Startup>();
        }
    }
}