using System.Collections.Generic;
using System.Linq;
using DynamicConfigurator.Persistence.Mongo.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DynamicConfigurator.Persistence.Mongo.Adapter
{
    public class MongoConfigurationRepository : IConfigurationRepository
    {
        public const string KeySelector = "_id";
        public const string ValueSelector = "value";
        public const string CollectionName = "configs";
        private const string DatabaseName = "configs";
        private const string Connection = "mongodb://localhost:27017";

        private readonly IMongoDatabase database;

        public MongoConfigurationRepository(string connection, string databaseName)
        {
            var mongoClient = new MongoClient(MongoClientSettings.FromUrl(new MongoUrl(connection ?? Connection)));
            database = mongoClient.GetDatabase(databaseName ?? DatabaseName);

            InitCollection();
        }

        private void InitCollection()
        {
            if (!database.CollectionExists(CollectionName))
            {
                database.CreateCollection(CollectionName);
            }
        }

        public void Create(string key, string value)
        {
            var collection = database.GetCollection(CollectionName);

            var document = new BsonDocument
            {
                [KeySelector] = key,
                [ValueSelector] = BsonDocument.Parse(value)
            };

            collection.ReplaceOne(p => p[KeySelector] == key, document, new UpdateOptions { IsUpsert = true });
        }

        public string Read(string key)
        {
            var collection = database.GetCollection(CollectionName);
            var document = collection.Find(x => x[KeySelector] == key).FirstOrDefault();

            return document?[ValueSelector].ToString();
        }

        public List<string> GetKeys()
        {
            var collection = database.GetCollection(CollectionName);
            return collection.AsQueryable()
                             .Select(document => (string)document[KeySelector])
                             .ToList();
        }

        public bool Delete(string key)
        {
            var collection = database.GetCollection(CollectionName);

            return collection.DeleteOne(x => x[KeySelector] == key).IsAcknowledged;
        }
    }
}
