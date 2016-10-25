using System.Collections.Generic;

namespace DynamicConfigurator.Server.Persistance
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

        public bool Delete(string key)
        {
            return repo.Remove(key);
        }
    }
}
