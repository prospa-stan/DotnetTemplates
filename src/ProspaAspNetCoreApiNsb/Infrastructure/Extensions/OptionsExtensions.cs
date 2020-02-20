using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddConfiguration<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : class, new()
        {
            services.Configure<TOptions>(configuration.GetSection(typeof(TOptions).Name));
            return services.AddSingleton(p => p.GetRequiredService<IOptions<TOptions>>().Value);
        }
    }
}