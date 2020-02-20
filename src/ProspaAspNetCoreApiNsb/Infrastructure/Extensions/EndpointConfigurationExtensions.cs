using System.Reflection;
using NServiceBus.Pipeline;
using ProspaAspNetCoreApiNsb.Behaviours;

// ReSharper disable CheckNamespace
namespace NServiceBus
// ReSharper restore CheckNamespace
{
    public static class EndpointConfigurationExtensions
    {
        public static PipelineSettings StripAssemblyVersionFromEnclosedMessageTypePipeline(this PipelineSettings pipeline)
        {
            pipeline.Register(
                behavior: new OutgoingHeaderBehavior(),
                description: "Strips assembly version from enclosed message type");

            return pipeline;
        }

        public static EndpointConfiguration UseLicence(
            this EndpointConfiguration endpointConfiguration,
            Assembly assembly)
        {
            endpointConfiguration.LicensePath(assembly.Location.Replace(assembly.GetName().Name + ".dll", "License.xml"));

            return endpointConfiguration;
        }
    }
}
