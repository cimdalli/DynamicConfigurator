using System;

namespace DynamicConfigurator.Client
{
    public delegate void ConfigHasChangedEventHandler(string config);

    public interface IConfigurationClient
    {
        event ConfigHasChangedEventHandler ConfigHasChanged;

        void NotifyConfigHasChanged(string config);

        Uri ConfigurationServerUri { get; }

        T GetConfiguration<T>(string application, string environment = null);

        void SetConfiguration(string application, object data, string environment = null);
    }
}