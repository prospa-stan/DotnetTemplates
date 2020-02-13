using Microsoft.AspNetCore.Mvc;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Options
    // ReSharper restore CheckNamespace
{
    public class ConventionsMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcOptions options)
        {
        }
    }
}
