using System.Collections.Generic;

namespace DynamicConfigurator.Server.Persistance
{
    public class InMemoryConfigurationRepository : IConfigurationRepository
    {
        readonly Dictionary<string, object> _repo;

        public InMemoryConfigurationRepository()
        {
            _repo = new Dictionary<string, object>();
        }

        public void Add<T>(string key, T value)
        {
            _repo.Add(key, value);
        }

        public T Get<T>(string key)
        {
            object value;
            _repo.TryGetValue(key, out value);
            return (T)value;
        }
    }
}
