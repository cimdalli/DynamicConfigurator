namespace DynamicConfigurator.Server.Persistance
{
    public interface IConfigurationRepository
    {
        void Add(string key, string value);
        string Get(string key);
        bool Remove(string key);
        void Update(string key, string value);
    }
}