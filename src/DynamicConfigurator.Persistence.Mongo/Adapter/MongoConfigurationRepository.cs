using MongoDB.Bson;
using MongoDB.Driver;

namespace DynamicConfigurator.Persistence.Adapter
{
    public class MongoConfigurationRepository : IConfigurationRepository
    {
        public static string Database = "configs";
        private readonly MongoClient _mongoClient;

        public MongoConfigurationRepository(string connectionString, string database)
        {
            Database = database ?? Database;
            connectionString = connectionString ?? "mongodb://localhost:27017";
            _mongoClient = new MongoClient(MongoClientSettings.FromUrl(new MongoUrl(connectionString)));
        }

        public void Create(string key, string value)
        {
            _mongoClient.GetDatabase(Database).CreateCollection(key, new CreateCollectionOptions
            {
                MaxDocuments = 1
            });

            var collection = GetCollection(key);
            var document = BsonDocument.Parse(value);

            collection.InsertOne(document);
        }

        public string Read(string key)
        {
            var collection = GetCollection(key);
            return collection.ToBsonDocument().ToString();
        }

        public bool Delete(string key)
        {
            _mongoClient.GetDatabase(Database).DropCollection(key);
            return true;
        }

        private IMongoCollection<BsonDocument> GetCollection(string key)
        {
            return _mongoClient
              .GetDatabase(Database)
              .GetCollection<BsonDocument>(key);
        }
    }
}
