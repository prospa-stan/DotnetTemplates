using System;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace ProspaWorker.Constants
{
    public static class Environment
    {
        public static string Prefix(IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment()
                || hostEnvironment.IsStaging())
            {
                return "staging-";
            }

            if (hostEnvironment.IsProduction())
            {
                return "live-";
            }

            throw new InvalidOperationException("Invalid DEPLOYMENT_ENVIRONMENT");
        }
    }
}
