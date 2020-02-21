using System;
using Destructurama;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ProspaWorker
{
    public static class ProgramLogger
    {
        public static void CreateDefaultLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            _ = loggerConfiguration
                .ReadFrom
                .Configuration(context.Configuration)
                .Enrich
                .WithDefaults()
                .Enrich
                .WithDemystifiedStackTraces()
                .Destructure.UsingAttributes();

            if (context.HostingEnvironment.EnvironmentName == Environments.Development)
            {
                WriteToConsole(loggerConfiguration);
            }

            WriteToDataDog(context.Configuration, loggerConfiguration, context.HostingEnvironment.EnvironmentName);
            _ = loggerConfiguration
                .WriteTo
                .ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Events, LogEventLevel.Warning);
        }

        private static void WriteToConsole(LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
        }

        private static void WriteToDataDog(IConfiguration configuration, LoggerConfiguration loggerConfiguration, string environment)
        {
            var dataDogApiKey = configuration.GetValue<string>("DataDogApiKey");

            if (!string.IsNullOrWhiteSpace(dataDogApiKey))
            {
                loggerConfiguration.WriteTo.DatadogLogs(
                    dataDogApiKey,
                    source: "<<source-name>>",
                    service: typeof(Program).Assembly.GetName().Name,
                    tags: new[] {$"env:{environment}", "p3domain:<<app-domain>>", "p3app:<<app-name>>"},
                    logLevel: LogEventLevel.Information);
            }

            if (Environment.GetEnvironmentVariable("DOTNET_CONSOLELOG") != null)
            {
                WriteToConsole(loggerConfiguration);
            }
        }
    }
}
