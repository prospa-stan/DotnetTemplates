using System;
using Microsoft.AspNetCore.HttpOverrides;
using static Microsoft.Extensions.Hosting.Environments;

namespace ProspaAspNetCoreApiNsb
{
    public static class Constants
    {
        public const string KeyVaultName = nameof(KeyVaultName);

        public static class Auth
        {
            public const string EndpointKey = nameof(EndpointKey);

            public static class Policies
            {
                public const string ReadPolicy = "Read";
                public const string WritePolicy = "Write";
            }
        }

        public static class ConnectionStrings
        {
            public static readonly string ServiceBus = nameof(ServiceBus);
        }

        public static class ConfigurationKeys
        {
            public static class AppInsights
            {
                public const string InstrumentationKey = "APPINSIGHTS_INSTRUMENTATIONKEY";
            }

            public static class Seq
            {
                public const string SeqServerUrl = nameof(SeqServerUrl);
            }
        }

        public static class Cors
        {
            public const string AllowAny = nameof(AllowAny);
        }

        public static class Environments
        {
            public static readonly string CurrentAspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            public static string Prefix()
            {
                if (IsDevelopment)
                {
                    return "demo-";
                }

                if (IsStaging)
                {
                    return "staging-";
                }

                if (IsProduction)
                {
                    return "live-";
                }

                throw new ApplicationException("Invalid ASPNETCORE_ENVIRONMENT");
            }

            public static bool IsDevelopment => CurrentAspNetCoreEnv == Development;

            public static bool IsStaging => CurrentAspNetCoreEnv == Staging;

            public static bool IsProduction => CurrentAspNetCoreEnv == Production;
        }

        public static class HttpHeaders
        {
            public const int HstsMaxAgeDays = 18 * 7;
            public const ForwardedHeaders ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All;
        }

        public static class Versioning
        {
            public const string GroupNameFormat = "'v'V";
        }
    }
}
