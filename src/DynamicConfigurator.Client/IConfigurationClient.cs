using System;

namespace DynamicConfigurator.Client
{
    public delegate void ConfigHasChangedEventHandler();

    public interface IConfigurationClient
    {
        event ConfigHasChangedEventHandler ConfigHasChanged;

        void NotifyConfigHasChanged();

        bool IsSameHost(string host);

        T GetConfiguration<T>(string application, string environment = null);

        void SetConfiguration(string application, object data, string environment = null);
    }
}