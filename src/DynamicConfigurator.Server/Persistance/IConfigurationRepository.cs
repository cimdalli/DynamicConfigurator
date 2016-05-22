namespace DynamicConfigurator.Server.Persistance
{
    public interface IConfigurationRepository
    {
        void Create(string key, string value);
        string Read(string key);
        void Update(string key, string value);
        bool Delete(string key);
    }
}