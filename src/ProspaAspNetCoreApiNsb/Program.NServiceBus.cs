using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using ProspaAspNetCoreApiNsb.Infrastructure;

namespace ProspaAspNetCoreApiNsb
{
    public static class ProgramNServiceBus
    {
        public static IHostBuilder UseDefaultNServiceBus(this IHostBuilder builder)
        {
            return builder.UseNServiceBus(context =>
            {
                var connectionString = context.Configuration.GetConnectionString(Constants.ConnectionStrings.ServiceBus);

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ApplicationException("ServiceBus Connection String must be provided");
                }

                var connection = new ServiceBusConnectionStringBuilder(context.Configuration.GetConnectionString(Constants.ConnectionStrings.ServiceBus));

                var logger = context.CreateDefaultLogger();
                var factory = LogManager.Use<SerilogFactory>();
                factory.WithLogger(logger);

                var endpointName = GetEndpointName();
                var cfg = new EndpointConfiguration(endpointName);

                cfg.UseLicence(typeof(Program).Assembly);
                cfg.SendFailedMessagesTo("error");
                cfg.AuditProcessedMessagesTo("audit");
                cfg.EnableInstallers();
                cfg.DisableFeature<AutoSubscribe>();

                var serialization = cfg.UseSerialization<NewtonsoftSerializer>();
                serialization.Settings(DefaultMessageJsonSerializerSettings.Instance);

                cfg.CustomDiagnosticsWriter(diagnostics => Task.CompletedTask);
                cfg.Recoverability()
                    .Immediate(c => c.NumberOfRetries(1))
                    .Delayed(c => c.NumberOfRetries(5).TimeIncrease(TimeSpan.FromSeconds(2)));

                cfg.UniquelyIdentifyRunningInstance().UsingNames(endpointName, Environment.MachineName);

                var pipeline = cfg.Pipeline;
                pipeline.StripAssemblyVersionFromEnclosedMessageTypePipeline();

                var transport = cfg.UseTransport<AzureServiceBusTransport>();
                transport.ConnectionString(connection.ToString);

                var routing = transport.Routing();
                // routing.RouteToEndpoint(typeof(EventType), "EndpointName");

                var serilog = cfg.EnableSerilogTracing(logger);
                serilog.EnableMessageTracing();

                return cfg;
            });
        }

        private static string GetEndpointName()
        {
            var endpointName = "ProspaAspNetCoreApiNsb";
            if (Constants.Environments.IsDevelopment)
            {
                endpointName = $"ProspaAspNetCoreApiNsb.{Environment.MachineName}";
            }

            return endpointName;
        }
    }
}
