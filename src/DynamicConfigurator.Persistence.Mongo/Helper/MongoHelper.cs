using MongoDB.Bson;
using MongoDB.Driver;

namespace DynamicConfigurator.Persistence.Mongo.Helper
{
    public static class MongoHelper
    {
        public static bool CollectionExists(this IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collections.Any();
        }

        public static void DropCollection(this IMongoDatabase database, string collectionName)
        {
            if (database.CollectionExists(collectionName))
            {
                database.DropCollection(collectionName);
            }
        }

        public static IMongoCollection<BsonDocument> GetCollection(this IMongoDatabase database, string collectionName)
        {
            return database.GetCollection<BsonDocument>(collectionName);
        }
    }
}
