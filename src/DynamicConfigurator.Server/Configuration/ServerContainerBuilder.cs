using Autofac;
using DynamicConfigurator.Persistence;
using DynamicConfigurator.Persistence.Adapter;
using DynamicConfigurator.Server.Api.Exceptions;
using DynamicConfigurator.Server.Configuration;
using DynamicConfigurator.Server.Notification;
using DynamicConfigurator.Server.Services;

namespace DynamicConfigurator.Server.Api.Configuration
{
    public class ServerContainerBuilder : ContainerBuilder
    {
        public ServerContainerBuilder()
        {
            var settings = ServerSettings.Current();

            RegisterRepository(settings);

            this.RegisterType<ConfigurationService>().AsSelf();
            this.RegisterType<HttpClientNotifier>().AsImplementedInterfaces();
            this.RegisterType<ServerErrorMapper>().AsImplementedInterfaces();
        }

        private void RegisterRepository(ServerSettings settings)
        {
            IConfigurationRepository repository = new InMemoryConfigurationRepository();

            if (settings?.Component?.Repository != null)
            {
                repository = (IConfigurationRepository)settings.Component.Repository.Create();
            }

            this.RegisterInstance(repository).AsImplementedInterfaces();
        }
    }
}
