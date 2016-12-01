using Autofac;
using DynamicConfigurator.Persistence.Mongo.Adapter;
using DynamicConfigurator.Persistence.Mongo.Helper;
using DynamicConfigurator.Server.Api.Configuration;
using FluentAssertions;
using MongoDB.Driver;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_storing_configs
{
    [TestFixture]
    public class with_mongo
    {
        private static Browser _server;
        private IMongoDatabase database;
        private dynamic sampleConfig;
        private bool isConfigExist;

        private const string Connection = "mongodb://localhost:27017";
        private const string DatabaseName = "config-integration";
        private const string CollectionName = MongoConfigurationRepository.CollectionName;
        private const string KeySelector = MongoConfigurationRepository.KeySelector;


        [SetUp]
        public void SetUp()
        {
            const string applicationName = "new";

            database = new MongoClient(Connection).GetDatabase(DatabaseName);
            database.DropCollection(CollectionName);

            var containerBuilder = new ServerContainerBuilder();

            containerBuilder.Register(c => new MongoConfigurationRepository(Connection, DatabaseName)).AsImplementedInterfaces();

            _server = new Browser(new ServerBootstrapper(containerBuilder));

            sampleConfig = TestHelper.GetSampleConfig();

            _server.Post($"application/{applicationName}", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });

            isConfigExist = database.GetCollection(CollectionName).Find(x => x[KeySelector] == applicationName).Any();
        }

        [TearDown]
        public void TearDown()
        {
            database.DropCollection(CollectionName);
        }

        [Test]
        public void config_file_should_be_created()
        {
            isConfigExist.Should().BeTrue();
        }
    }
}
