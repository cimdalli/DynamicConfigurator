using System.Collections.Generic;
using System.Linq;

namespace DynamicConfigurator.Persistence.Adapter
{
    public class InMemoryConfigurationRepository : IConfigurationRepository
    {
        private readonly Dictionary<string, string> repo;

        public InMemoryConfigurationRepository()
        {
            repo = new Dictionary<string, string>();
        }

        public void Create(string key, string value)
        {
            repo.Add(key, value);
        }

        public string Read(string key)
        {
            string value;
            repo.TryGetValue(key, out value);
            return value;
        }

        public List<string> GetKeys()
        {
            return repo.Keys.ToList();
        }

        public bool Delete(string key)
        {
            return repo.Remove(key);
        }
    }
}
