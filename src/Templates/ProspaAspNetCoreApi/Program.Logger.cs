using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ProspaAspNetCoreApi
{
    public static class ProgramLogger
    {
        public static void ConfigureLogger(WebHostBuilderContext context, LoggerConfiguration loggerConfiguration)
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

            if (Constants.Environments.IsDevelopment())
            {
                loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate);
            }
            else
            {
                loggerConfiguration
                    .WriteTo
                    .ApplicationInsightsTraces(
                        context.Configuration.GetValue<string>(Constants.ConfigurationKeys.AppInsights.InstrumentationKey),
                        restrictedToMinimumLevel: LogEventLevel.Error);
            }
        }
    }
}