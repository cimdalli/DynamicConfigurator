namespace DynamicConfigurator.Server.Persistance
{
    public interface IConfigurationRepository
    {
        void Add<T>(string key, T value);
        T Get<T>(string key);
    }
}