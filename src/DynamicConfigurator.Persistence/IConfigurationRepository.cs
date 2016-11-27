namespace DynamicConfigurator.Persistence
{
    public interface IConfigurationRepository
    {
        void Create(string key, string value);
        string Read(string key);
        bool Delete(string key);
    }
}