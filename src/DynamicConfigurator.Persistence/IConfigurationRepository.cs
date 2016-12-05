using System.Collections.Generic;

namespace DynamicConfigurator.Persistence
{
    public interface IConfigurationRepository
    {
        void Create(string key, string value);
        string Read(string key);
        List<string> GetKeys();
        bool Delete(string key);
    }
}