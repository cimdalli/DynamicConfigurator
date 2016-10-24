using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using DynamicConfigurator.Server.Configuration;
using DynamicConfigurator.Server.Persistance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server
{
    public class ConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly Formatting _formatting;
        private readonly string _systemKey;
        private readonly string _defaultKey;
        private const string EmptyObject = "{}";


        public ConfigurationService(IConfigurationRepository configurationRepository, ServerSettings settings)
        {
            _configurationRepository = configurationRepository;
            _formatting = settings.App.FormattingValue;
            _systemKey = settings.App.SystemKey ?? "system";
            _defaultKey = settings.App.DefaultKey ?? "default";
        }


        public void Set(string application, JObject value, string environment = null)
        {
            var configString = _configurationRepository.Read(application);
            var allConfig = JObject.Parse(configString ?? EmptyObject);
            allConfig[environment ?? _defaultKey] = value;

            //new config
            if (configString == null)
            {
                _configurationRepository.Create(application, allConfig.ToString(_formatting));
            }
            else
            {
                _configurationRepository.Update(application, allConfig.ToString(_formatting));
                ConfigChanged(application, environment);
            }
        }


        public JObject Get(string application, string environment = null, string client = null)
        {
            var configString = _configurationRepository.Read(application);
            if (configString == null)
                return null;

            var allConfig = JObject.Parse(configString);
            var defaultConfig = JObject.Parse(EmptyObject);

            if (allConfig[_defaultKey] != null)
            {
                defaultConfig = allConfig[_defaultKey].Value<JObject>();
            }

            if (environment == null)
                return defaultConfig;

            var environmentConfig = allConfig[environment];
            defaultConfig.Merge(environmentConfig);

            return defaultConfig;
        }

        public void Subscribe(string application, string environment, string client)
        {
            var systemConfig = GetSystemConfig();
            var key = CreateRegisterKey(application, environment);
            List<string> clients;

            if (systemConfig.RegisteredClients.TryGetValue(key, out clients))
            {
                if (!clients.Contains(client))
                {
                    clients.Add(client);
                    SaveSystemConfig(systemConfig);
                }
            }
        }


        public void ConfigChanged(string application, string environment)
        {
            var systemConfig = GetSystemConfig();
            var key = CreateRegisterKey(application, environment);
            List<string> clients;

            if (systemConfig.RegisteredClients.TryGetValue(key, out clients))
            {
                clients.ForEach(client =>
                {
                    new HttpClient
                    {
                        BaseAddress = new Uri(client)
                    }
                    .PostAsync("notify", null);
                });
            }
        }

        private static string CreateRegisterKey(params string[] variables)
        {
            return string.Join(".", variables.Where(s => !string.IsNullOrEmpty(s)));
        }

        private SystemConfig GetSystemConfig()
        {
            var systemConfig = Get(_systemKey) ?? JObject.FromObject(new SystemConfig());

            return systemConfig.ToObject<SystemConfig>();
        }

        private void SaveSystemConfig(SystemConfig systemConfig)
        {
            Set(_systemKey, JObject.FromObject(systemConfig));
        }
    }
}
