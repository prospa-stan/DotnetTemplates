using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace ProspaAspNetCoreApi
{
    public static class ProgramConfiguration
    {
        public static IWebHostBuilder ConfigureDefaultAppConfiguration(this IWebHostBuilder webHostBuilder, string[] args)
        {
            webHostBuilder.ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.AddDefaultSources(args);
                });

            return webHostBuilder;
        }

        public static IConfigurationBuilder AddDefaultSources(this IConfigurationBuilder builder, string[] args = null)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Constants.Environments.CurrentAspNetCoreEnv ?? Constants.Environments.Production}.json", optional: true)
                .AddEnvironmentVariables();

            if (Constants.Environments.IsDevelopment())
            {
                // config.AddUserSecrets(Assembly.GetExecutingAssembly());
            }

            AddDefaultAzureKeyVault(builder);

            if (args != null)
            {
                builder.AddCommandLine(args);
            }

            return builder;
        }

        private static void AddDefaultAzureKeyVault(IConfigurationBuilder builder)
        {
            var builtConfig = builder.Build();
            var keyvaultName = builtConfig.GetValue<string>("keyvaultName");

            if (string.IsNullOrEmpty(keyvaultName))
            {
                return;
            }

            var keyVaultEndpoint = $"https://{Constants.Environments.Prefix()}{keyvaultName}.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }
    }
}