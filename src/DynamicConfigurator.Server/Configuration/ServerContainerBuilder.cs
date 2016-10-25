﻿using Autofac;
using DynamicConfigurator.Server.Exceptions;
using DynamicConfigurator.Server.Notification;
using DynamicConfigurator.Server.Persistance;

namespace DynamicConfigurator.Server.Configuration
{
    public class ServerContainerBuilder : ContainerBuilder
    {
        public ServerContainerBuilder() : this(ServerSettings.Current())
        {
        }

        public ServerContainerBuilder(ServerSettings settings)
        {
            this.RegisterInstance(settings).SingleInstance();

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
                repository = settings.Component.Repository.Create<IConfigurationRepository>();
            }

            this.RegisterInstance(repository).AsImplementedInterfaces();
        }
    }
}
