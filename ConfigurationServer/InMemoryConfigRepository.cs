using System.Collections.Generic;

namespace ConfigurationServer
{
    public class InMemoryConfigRepository : ConfigRepository
    {
        readonly Dictionary<string, dynamic> _repo;

        public InMemoryConfigRepository()
        {
            _repo = new Dictionary<string, dynamic>();
        }

        public override void Add<T>(string key, T value)
        {
            _repo.Add(key, value);
        }

        public override T Get<T>(string key)
        {
            object value;
            _repo.TryGetValue(key, out value);
            return (T)value;
        }
    }
}
