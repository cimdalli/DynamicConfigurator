using DynamicConfigurator.Server.Persistance;
using Nancy;
using Nancy.TinyIoc;

namespace DynamicConfigurator.Server.Configuration
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<InMemoryConfigurationRepository>();
            container.Register<ConfigurationManager>();

            base.ConfigureApplicationContainer(container);
        }
    }
}
