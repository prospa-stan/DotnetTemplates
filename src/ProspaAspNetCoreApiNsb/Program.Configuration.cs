using System;
using System.IO;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace ProspaAspNetCoreApiNsb
{
    public static class ProgramConfiguration
    {
        public static IConfigurationBuilder AddDefaultSources(this IConfigurationBuilder builder, string[] args = null)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Constants.Environments.CurrentAspNetCoreEnv ?? Environments.Production}.json", optional: true)
                .AddEnvironmentVariables();

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
            var keyVaultName = builtConfig.GetValue<string>(Constants.KeyVaultName);

            if (string.IsNullOrEmpty(keyVaultName))
            {
                throw new ApplicationException("A Keyvault name but be present in application settings and the matching Keyvault resource needs to exist. Use az login to authenticate with Keyvault with MSI, ensure that you have permissions to list Keyvault keys");
            }

            var keyVaultEndpoint = $"https://{Constants.Environments.Prefix()}{keyVaultName}.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }
    }
}
