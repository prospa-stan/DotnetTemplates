using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace ProspaAspNetCoreApi.ConfigureOptions
{
    public class CorsOptionsSetup : IConfigureOptions<CorsOptions>
    {
        /// <inheritdoc />
        public void Configure(CorsOptions options)
        {
            options.AddPolicy(
                Constants.Cors.AllowAny,
                x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("WWW-Authenticate"));
        }
    }
}
