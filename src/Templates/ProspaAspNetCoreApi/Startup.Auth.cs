using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prospa.Extensions.AspNetCore.Authorization;

namespace ProspaAspNetCoreApi
{
    public static class StartupAuth
    {
        public static IServiceCollection AddDefaultAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<AuthOptions>>().Value);

            services.AddDefaultAuthorization();
            services.AddDefaultAuthentication(configuration);

            return services;
        }

        public static IServiceCollection AddDefaultAuthorization(this IServiceCollection services)
        {
            services
                .AddDefaultScopeAuthorization();

            return services;
        }

        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = new AuthOptions();
            configuration.GetSection(nameof(AuthOptions)).Bind(authOptions);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(
                options =>
                {
                    options.Authority = authOptions.Authority;
                    options.ApiName = authOptions.Audience;
                    options.SupportedTokens = SupportedTokens.Both;
                    options.JwtValidationClockSkew = TimeSpan.FromSeconds(30);
                });

            return services;
        }

        public static IServiceCollection AddDefaultScopeAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ScopeAuthorizationOptionsSetup>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            return services;
        }
    }
}
