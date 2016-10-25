using DynamicConfigurator.Server.Configuration;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_creating_a_config
{
    [TestFixture]
    public class without_environment_data
    {
        private static Browser _server;

        private BrowserResponse getEmptyConfigResponse;
        private BrowserResponse setConfigResponse;
        private BrowserResponse getNewCreatedConfigResponse;
        private dynamic getNewCreatedConfigResult;
        private dynamic sampleConfig;

        [SetUp]
        public void SetUp()
        {
            _server = new Browser(new ServerBootstrapper());

            sampleConfig = TestHelper.GetSampleConfig();

            getEmptyConfigResponse = _server.Get("application/empty");

            setConfigResponse = _server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });

            getNewCreatedConfigResponse = _server.Get("application/new");

            getNewCreatedConfigResult = getNewCreatedConfigResponse.Body.AsJson();
        }

        [Test]
        public void non_exist_config_should_be_empty()
        {
            getEmptyConfigResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void new_config_should_be_set_successfully()
        {
            setConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void created_config_should_be_gotten_successfully()
        {
            getNewCreatedConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void created_config_should_have_correct_values()
        {
            string application = getNewCreatedConfigResult.application;
            string mongoUrl = getNewCreatedConfigResult.persistence.mongo.url;

            application.Should().Be(sampleConfig.Application);
            mongoUrl.Should().Be(sampleConfig.Persistence.Mongo.Url);
        }
    }
}
