using FluentAssertions;
using NUnit.Framework;

namespace DynamicConfigurator.Client.Test.When_creating_a_config
{
    [TestFixture]
    public class that_is_a_new_config
    {
        private object _nonExistConfig;
        private SampleConfigData _getNewCreatedConfig;
        private SampleConfigData _sampleConfig;

        [SetUp]
        public void SetUp()
        {
            var configurationClient = new ConfigurationClient(ApiTestFixture.ConfigurationServerUri);

            _sampleConfig = GenerateSampleConfigData();

            _nonExistConfig = configurationClient.GetConfiguration<object>("non-exist");

            configurationClient.SetConfiguration("application", _sampleConfig);

            _getNewCreatedConfig = configurationClient.GetConfiguration<SampleConfigData>("application");
        }

        [Test]
        public void getting_non_exist_config_should_be_empty()
        {
            _nonExistConfig.Should().BeNull();
        }

        [Test]
        public void getting_new_created_config_should_be_successfully()
        {
            _getNewCreatedConfig.Should().NotBeNull();
        }

        [Test]
        public void getting_new_created_config_should_have_correct_values()
        {
            var application = _getNewCreatedConfig.Application;
            var mongoUrl = _getNewCreatedConfig.Persistence.Mongo.Url;

            application.Should().Be(_sampleConfig.Application);
            mongoUrl.Should().Be(_sampleConfig.Persistence.Mongo.Url);
        }

        private SampleConfigData GenerateSampleConfigData()
        {
            return new SampleConfigData
            {
                Application = "testApp",
                Persistence = new PersistenceSettings
                {
                    Mongo = new DbSettings
                    {
                        Url = "mongoUrl"
                    }
                },
                ShutdownThreshold = 3
            };
        }
    }
}
