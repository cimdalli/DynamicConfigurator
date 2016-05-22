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

        public void Create(string key, string value)
        {
            _repo.Add(key, value);
        }

        public string Read(string key)
        {
            string value;
            _repo.TryGetValue(key, out value);
            return value;
        }

        public void Update(string key, string value)
        {
            Delete(key);
            Create(key, value);
        }

        public bool Delete(string key)
        {
            return _repo.Remove(key);
        }
    }
}
