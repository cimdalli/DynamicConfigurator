using Autofac;
using DynamicConfigurator.Common.Configuration;
using DynamicConfigurator.Common.Domain;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Serialization.JsonNet;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Configuration
{
    public class ServerBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ContainerBuilder _containerBuilder;

        public ServerBootstrapper()
            : this(new ServerContainerBuilder())
        {
        }

        public ServerBootstrapper(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            pipelines.EnableJsonErrorResponse(container.Resolve<IErrorMapper>());

            CreateSystemConfig(container.Resolve<ConfigurationService>());

            base.ApplicationStartup(container, pipelines);
        }


        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            _containerBuilder.Update(existingContainer.ComponentRegistry);

            base.ConfigureApplicationContainer(existingContainer);
        }


        private static void CreateSystemConfig(ConfigurationService configurationService)
        {
            var defaultSystemConfig = JObject.FromObject(new SystemConfiguration());

            configurationService.GetOrCreate("system", defaultSystemConfig);
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
