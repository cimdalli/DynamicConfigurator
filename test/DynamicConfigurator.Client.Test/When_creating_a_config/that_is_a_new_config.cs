using System;
using FluentAssertions;
using NUnit.Framework;

namespace DynamicConfigurator.Client.Test.When_creating_a_config
{
    [TestFixture]
    public class that_is_a_new_config
    {
        private Action gettingNonExistConfig;
        private SampleConfigData getNewCreatedConfig;
        private SampleConfigData sampleConfig;

        [SetUp]
        public void SetUp()
        {
            var configurationClient = new ConfigurationClient(ApiTestFixture.ConfigurationServerUri);

            sampleConfig = GenerateSampleConfigData();

            gettingNonExistConfig = () => configurationClient.GetConfiguration<object>("non-exist");

            configurationClient.SetConfiguration("application", sampleConfig);

            getNewCreatedConfig = configurationClient.GetConfiguration<SampleConfigData>("application");
        }

        [Test]
        public void getting_non_exist_config_should_throw_exception()
        {
            gettingNonExistConfig.ShouldThrow<ConfigNotFoundException>();
        }

        [Test]
        public void getting_new_created_config_should_be_successfully()
        {
            getNewCreatedConfig.Should().NotBeNull();
        }

        [Test]
        public void getting_new_created_config_should_have_correct_values()
        {
            var application = getNewCreatedConfig.Application;
            var mongoUrl = getNewCreatedConfig.Persistence.Mongo.Url;

            application.Should().Be(sampleConfig.Application);
            mongoUrl.Should().Be(sampleConfig.Persistence.Mongo.Url);
        }

        private static SampleConfigData GenerateSampleConfigData()
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
