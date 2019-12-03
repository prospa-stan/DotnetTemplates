using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ProspaAspNetCoreApi
{
    public partial class Program
    {
        private static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                                .ReadFrom
                                .Configuration(context.Configuration)
                                .Enrich
                                .WithDefaults();

            var seqServerUrl = context.Configuration.GetValue<string>(Constants.ConfigurationKeys.Seq.SeqServerUrl);

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
        }
    }
}