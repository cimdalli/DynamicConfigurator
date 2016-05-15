using DynamicConfigurator.Common.Configuration;
using DynamicConfigurator.Common.Domain;
using DynamicConfigurator.Server.Persistance;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Serialization.JsonNet;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Configuration
{
    public class ServerBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.EnableJsonErrorResponse(container.Resolve<IErrorMapper>());

            CreateSystemConfig(container.Resolve<ConfigurationManager>());

            base.ApplicationStartup(container, pipelines);
        }

        private static void CreateSystemConfig(ConfigurationManager configurationManager)
        {
            var defaultSystemConfig = JObject.FromObject(new SystemConfiguration());

            configurationManager.GetOrCreate("system", defaultSystemConfig);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<InMemoryConfigurationRepository>();
            container.Register<ConfigurationManager>();
            container.Register<ServerErrorMapper>();

            base.ConfigureApplicationContainer(container);
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration
                      .WithOverrides(nic =>
                      {
                          nic.Serializers.Clear();
                          nic.Serializers.Insert(0, typeof(JsonNetSerializer));
                      });
            }
        }
    }
}
