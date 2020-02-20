using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ProspaAspNetCoreApiNsb
{
    public static partial class Program
    {
        public static ILogger CreateDefaultLogger(this HostBuilderContext context)
        {
            var loggerConfiguration = new LoggerConfiguration();
            SetLoggerConfiguration(loggerConfiguration, context.Configuration);

            return loggerConfiguration.CreateLogger();
        }

        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            SetLoggerConfiguration(loggerConfiguration, context.Configuration);
        }

        private static LoggerConfiguration SetLoggerConfiguration(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration
                .ReadFrom
                .Configuration(configuration)
                .Enrich
                .WithDefaults();

            var seqServerUrl = configuration.GetValue<string>(Constants.ConfigurationKeys.Seq.SeqServerUrl);

            if (!string.IsNullOrWhiteSpace(seqServerUrl))
            {
                loggerConfiguration.WriteTo.Seq(seqServerUrl);
            }

            loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate);

            loggerConfiguration
                .WriteTo
                .ApplicationInsights(
                TelemetryConverter.Traces,
                restrictedToMinimumLevel: LogEventLevel.Information);

            return loggerConfiguration;
        }
    }
}
