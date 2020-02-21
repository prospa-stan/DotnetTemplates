using System;
using System.Reflection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace ProspaWorker
{
    public static class ProgramConfiguration
    {
        public static IHostBuilder ConfigureDefaultAppConfiguration(this IHostBuilder hostBuilder)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.AddDefaultSources(context);
                });

            return hostBuilder;
        }

        public static IConfigurationBuilder AddDefaultSources(
            this IConfigurationBuilder builder,
            HostBuilderContext context)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.HostingEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets(Assembly.GetExecutingAssembly());
            }
            else
            {
                AddDefaultAzureKeyVault(builder, context.HostingEnvironment);
            }

            return builder;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "This object can't be disposed")]
        private static void AddDefaultAzureKeyVault(IConfigurationBuilder builder, IHostEnvironment hostEnvironment)
        {
            var builtConfig = builder.Build();
            var keyVaultName = builtConfig.GetValue<string>("keyVaultName");

            if (keyVaultName == null)
            {
                return;
            }

            var keyVaultEndpoint = $"https://{Constants.Environment.Prefix(hostEnvironment)}{keyVaultName}.vault.azure.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }
    }
}
