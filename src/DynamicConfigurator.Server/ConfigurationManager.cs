using System;
using System.Collections.Generic;
using DynamicConfigurator.Server.Persistance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server
{
    public class ConfigurationManager
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly Dictionary<string, List<Action>> _subscribers;
        private const string EmptyObject = "{}";

        public ConfigurationManager(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
            _subscribers = new Dictionary<string, List<Action>>();
        }

        public void Set(string application, JObject value, string environment = null)
        {
            var configString = _configurationRepository.Get(application);
            var allConfig = JObject.Parse(configString ?? EmptyObject);
            allConfig[environment ?? "default"] = value;

            //new config
            if (configString == null)
            {
                _configurationRepository.Add(application, allConfig.ToString(Formatting.None));
            }
            else
            {
                _configurationRepository.Update(application, allConfig.ToString(Formatting.None));
                ConfigChanged(application, environment);
            }
        }

        public JObject Get(string application, string environment = null)
        {
            var configString = _configurationRepository.Get(application);
            if (configString != null)
            {
                var allConfig = JObject.Parse(configString);
                var defaultConfig = JObject.Parse(EmptyObject);

                if (allConfig["default"] != null)
                {
                    defaultConfig = allConfig["default"].Value<JObject>();
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
            string channel = application + (environment != null ? ("." + environment) : null);

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

            if (environment != null && _subscribers.TryGetValue($"{application}.{environment}", out actionList))
            {
                actionList.ForEach(action => action());
            }
            else if (_subscribers.TryGetValue(application, out actionList))
            {
                actionList.ForEach(action => action());
            }
        }
    }
}
