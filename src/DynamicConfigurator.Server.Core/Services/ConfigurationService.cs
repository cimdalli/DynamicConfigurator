using System.Collections.Generic;
using System.Linq;
using DynamicConfigurator.Persistence;
using DynamicConfigurator.Server.Configuration;
using DynamicConfigurator.Server.Notification;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Services
{
    public class ConfigurationService
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IClientNotifier clientNotifier;
        private readonly string systemKey;
        private readonly string defaultKey;
        private const string EmptyObject = "{}";


        public ConfigurationService(
            IConfigurationRepository configurationRepository,
            IClientNotifier clientNotifier)
        {
            this.configurationRepository = configurationRepository;
            this.clientNotifier = clientNotifier;
            var settings = ServerSettings.Current();

            systemKey = settings.App.SystemKey ?? "system";
            defaultKey = settings.App.DefaultKey ?? "default";
        }


        public void SetConfig(string application, JObject value, string environment = null)
        {
            var existingConfig = configurationRepository.Read(application);
            var allConfig = JObject.Parse(existingConfig ?? EmptyObject);
            allConfig[environment ?? defaultKey] = value;

            configurationRepository.Delete(application);
            configurationRepository.Create(application, allConfig.ToString(Formatting.Indented));

            ConfigChanged(application, environment);
        }


        public JObject GetConfig(string application, string environment = null, string client = null)
        {
            var configString = configurationRepository.Read(application);
            if (configString == null)
                return null;

            var allConfig = JObject.Parse(configString);
            var defaultConfig = JObject.Parse(EmptyObject);

            if (allConfig[defaultKey] != null)
            {
                defaultConfig = allConfig[defaultKey].Value<JObject>();
            }

            if (environment != null)
            {
                var environmentConfig = allConfig[environment];
                defaultConfig.Merge(environmentConfig);
            }

            if (client != null)
            {
                Subscribe(application, environment, client);
            }

            return defaultConfig;
        }


        public SystemConfig GetSystemConfig()
        {
            var systemConfig = GetConfig(systemKey);

            return systemConfig == null ? new SystemConfig() : systemConfig.ToObject<SystemConfig>();
        }


        public void SaveSystemConfig(SystemConfig systemConfig)
        {
            SetConfig(systemKey, JObject.FromObject(systemConfig));
        }


        private void Subscribe(string application, string environment, string client)
        {
            var systemConfig = GetSystemConfig();
            var key = CreateRegisterKey(application, environment);

            if (!systemConfig.RegisteredClients.ContainsKey(key))
            {
                systemConfig.RegisteredClients.Add(key, new List<string>());
            }

            var clients = systemConfig.RegisteredClients[key];

            if (clients.Contains(client))
                return;

            clients.Add(client);
            clients.Sort();

            SaveSystemConfig(systemConfig);
        }

        private void ConfigChanged(string application, string environment)
        {
            var systemConfig = GetSystemConfig();
            var key = CreateRegisterKey(application, environment);
            var clients = systemConfig.RegisteredClients
                .Where(pair => pair.Key.StartsWith(key))
                .SelectMany(pair => pair.Value)
                .ToList();

            clients.ForEach(clientNotifier.NotifyClient);
        }

        private static string CreateRegisterKey(params string[] variables)
        {
            return string.Join(".", variables.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}
