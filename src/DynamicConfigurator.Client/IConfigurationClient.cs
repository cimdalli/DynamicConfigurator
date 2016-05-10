﻿using System;
using DynamicConfigurator.Common.Domain;

namespace DynamicConfigurator.Client
{
    public interface IConfigurationClient
    {
        event ConfigHasChangedEventHandler ConfigHasChanged;

        void NotifyConfigHasChanged();

        Uri ConfigurationServerUri { get; }

        T GetConfiguration<T>(string application, string environment = null) where T : class, IConfigData, new();

        //object GetConfiguration(string application, string environment = null);

        void SetConfiguration(string application, object data, string environment = null);
    }
}