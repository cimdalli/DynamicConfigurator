namespace DynamicConfigurator.Persistence.Adapter
{
    public class MongoConfigurationRepository : IConfigurationRepository
    {
        public const string CollectionName = "configs";

        public MongoConfigurationRepository()
        {
        }

        public void Create(string key, string value)
        {

        }

        public string Read(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
