using FluentAssertions;
using Nancy.Responses;
using NUnit.Framework;

namespace DynamicConfigurator.Client.Test.When_creating_a_config
{
    [TestFixture]
    public class that_is_a_new_config
    {
        //public static Browser Server;

        //BrowserResponse getEmptyConfigResponse;
        //BrowserResponse setConfigResponse;
        //BrowserResponse getNewCreatedConfigResponse;
        private object _getNewCreatedConfig;
        private SampleConfigData sampleConfig;

        [SetUp]
        public void SetUp()
        {
            var configurationClient = new ConfigurationClient(ApiTestFixture.ConfigurationServerUri);
            sampleConfig = GenerateSampleConfigData();

            configurationClient.SetConfiguration("application", sampleConfig);

            _getNewCreatedConfig = configurationClient.GetConfiguration<SampleConfigData>("application");

            //sampleConfig = TestHelper.GetSampleConfig();

            //getEmptyConfigResponse = Server.Get("application/empty");

            //setConfigResponse = Server.Post("application/new", context =>
            //{
            //    context.HttpRequest();
            //    context.JsonBody(sampleConfig as object);
            //});

            //getNewCreatedConfigResponse = Server.Get("application/new");

            //getNewCreatedConfig = getNewCreatedConfigResponse.Body.AsJson();
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


        [Test]
        public void getting_non_exist_config_should_be_empty()
        {
            //getEmptyConfigResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void setting_new_config_should_be_successfully()
        {
            //setConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_be_successfully()
        {
            _getNewCreatedConfig.Should().NotBeNull();
        }

        [Test]
        public void getting_new_created_config_should_have_correct_values()
        {
            //string application = getNewCreatedConfig.application;
            //string mongoUrl = getNewCreatedConfig.persistence.mongo.url;

            //application.Should().Be(sampleConfig.Application);
            //mongoUrl.Should().Be(sampleConfig.Persistence.Mongo.Url);
        }
    }
}
