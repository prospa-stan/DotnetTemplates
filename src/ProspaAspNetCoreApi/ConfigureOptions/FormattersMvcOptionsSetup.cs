using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace ProspaAspNetCoreApi.ConfigureOptions
{
    public class FormattersMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcOptions options)
        {
            // Not useful for an API serving JSON.
            options.OutputFormatters.RemoveType<StreamOutputFormatter>();
            options.OutputFormatters.RemoveType<StringOutputFormatter>();
        }
    }
}