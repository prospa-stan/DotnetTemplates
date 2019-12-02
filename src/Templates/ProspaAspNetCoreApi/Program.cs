using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
                    if (!string.IsNullOrEmpty(Constants.KeyVaultName))
                    {
                        var keyVaultEndpoint = $"https://{Constants.Environments.Prefix()}{Constants.KeyVaultName}.vault.azure.net/";
                        // builder.AddAzureKeyVault(keyVaultEndpoint);
                    }
                })
                .UseSerilog(ConfigureLogger)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder
                        .ConfigureKestrel(options =>
                        {
                            options.AddServerHeader = false;
                        })
                        .UseStartup<Startup>();
                });
    }
}
