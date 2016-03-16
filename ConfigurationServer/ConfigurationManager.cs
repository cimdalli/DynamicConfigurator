using DynamicConfigurator.Server.Domain.Exceptions;
using DynamicConfigurator.Server.Persistance;

namespace DynamicConfigurator.Server
{
    public class ConfigurationManager
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationManager(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public void Add<T>(string application, T value)
        {
            if (_configurationRepository.Get<T>(application) != null)
            {
                throw new ApplicationAlreadyAddedException();
            }
            _configurationRepository.Add(application, value);
        }

        public object Get(string application)
        {
            return _configurationRepository.Get<object>(application);
        }

        public T GetOrCreate<T>(string application, T defaultValue)
        {
            var returnValue = _configurationRepository.Get<T>(application);
            if (returnValue == null)
            {
                _configurationRepository.Add(application, defaultValue);
                returnValue = defaultValue;
            }
            return returnValue;
        }
    }
}
