using System;
using System.Collections.Generic;
using System.Linq;
using DynamicConfigurator.Common.Domain;
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
        private readonly Dictionary<string, List<Action>> _subscribers;
        private const string EmptyObject = "{}";


        public ConfigurationService(IConfigurationRepository configurationRepository, ServerSettings settings)
        {
            _configurationRepository = configurationRepository;
            _formatting = settings.App.FormattingValue;
            _systemKey = settings.App.SystemKey ?? "system";
            _defaultKey = settings.App.DefaultKey ?? "default";
            _subscribers = new Dictionary<string, List<Action>>();

            GetOrCreate(_systemKey, JObject.FromObject(new SystemConfiguration()));
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


        public JObject Get(string application, string environment = null)
        {
            var configString = _configurationRepository.Read(application);
            if (configString != null)
            {
                var allConfig = JObject.Parse(configString);
                var defaultConfig = JObject.Parse(EmptyObject);

                if (allConfig[_defaultKey] != null)
                {
                    defaultConfig = allConfig[_defaultKey].Value<JObject>();
                }

                if (environment != null)
                {
                    var environmentConfig = allConfig[environment];
                    defaultConfig.Merge(environmentConfig);
                }

                return defaultConfig;
            }

            return null;
        }


        //public SystemConfiguration GetSystemConfiguration()
        //{
        //    return Get(_systemKey).ToObject<SystemConfiguration>();
        //}


        public JObject GetOrCreate(string application, JObject defaultValue, string environment = null)
        {
            var returnValue = Get(application, environment);

            if (returnValue == null)
            {
                Set(application, defaultValue, environment);
                returnValue = defaultValue;
            }
            return returnValue;
        }


        public void Subscribe(string application, Action action)
        {
            Subscribe(application, null, action);
        }


        public void Subscribe(string application, string environment, Action action)
        {
            List<Action> actionList;
            var channel = CreateChannelName(application, environment);

            if (!_subscribers.TryGetValue(channel, out actionList))
            {
                actionList = new List<Action>();
                _subscribers[channel] = actionList;
            }
            actionList.Add(action);
        }


        public void ConfigChanged(string application, string environment)
        {
            List<Action> actionList;
            var channel = CreateChannelName(application, environment);

            if (_subscribers.TryGetValue(channel, out actionList))
            {
                actionList.ForEach(action => action());
            }
        }

        private static string CreateChannelName(params string[] variables)
        {
            return string.Join(".", variables.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}
