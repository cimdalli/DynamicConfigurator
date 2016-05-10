using System;
using DynamicConfigurator.Common.Configuration;
using DynamicConfigurator.Server.Persistance;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace DynamicConfigurator.Server.Configuration
{
    public class ServerBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.EnableJsonErrorResponse(container.Resolve<IErrorMapper>());

            base.ApplicationStartup(container, pipelines);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<InMemoryConfigurationRepository>();
            container.Register<ConfigurationManager>();
            container.Register<ServerErrorMapper>();
            container.Register<JsonSerializer, CustomJsonSerializer>();

            base.ConfigureApplicationContainer(container);
        }
    }
}
