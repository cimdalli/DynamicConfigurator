using System.Collections.Generic;

namespace DynamicConfigurator.Server.Persistance
{
    public class InMemoryConfigurationRepository : IConfigurationRepository
    {
        private readonly Dictionary<string, string> _repo;

        public InMemoryConfigurationRepository()
        {
            _repo = new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
            _repo.Add(key, value);
        }

        public string Get(string key)
        {
            string value;
            _repo.TryGetValue(key, out value);
            return value;
        }

        public bool Remove(string key)
        {
            return _repo.Remove(key);
        }

        public void Update(string key, string value)
        {
            _repo.Remove(key);
            _repo.Add(key, value);
        }
    }
}
