using Autofac;
using DynamicConfigurator.Server.Api.Exceptions;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Serialization.JsonNet;

namespace DynamicConfigurator.Server.Api.Configuration
{
    public class ServerBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ContainerBuilder containerBuilder;

        public ServerBootstrapper() : this(new ServerContainerBuilder())
        {
        }

        public ServerBootstrapper(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            pipelines.EnableJsonErrorResponse(container.Resolve<IErrorMapper>());
            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            containerBuilder.Update(existingContainer.ComponentRegistry);
            base.ConfigureApplicationContainer(existingContainer);
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(nic =>
                {
                    nic.Serializers.Clear();
                    nic.Serializers.Insert(0, typeof(JsonNetSerializer));
                });
            }
        }
    }
}
