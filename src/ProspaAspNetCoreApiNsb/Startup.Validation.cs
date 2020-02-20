using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProspaAspNetCoreApiNsb;
using ProspaAspNetCoreApiNsb.ConfigureOptions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class StartupValidation
    {
        public static IMvcBuilder AddDefaultValidation(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

            builder.Services.AddSingleton<IConfigureOptions<ApiBehaviorOptions>, ProblemJsonApiBehaviourOptionsSetup>();

            return builder;
        }
    }
}
