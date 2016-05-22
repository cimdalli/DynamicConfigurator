using DynamicConfigurator.Server.Configuration;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_creating_a_config
{
    [TestFixture]
    public class that_is_a_new_config
    {
        public static Browser Server;

        private BrowserResponse _getEmptyConfigResponse;
        private BrowserResponse _setConfigResponse;
        private BrowserResponse _getNewCreatedConfigResponse;
        private dynamic _getNewCreatedConfigResult;
        private dynamic _sampleConfig;

        [SetUp]
        public void SetUp()
        {
            Server = new Browser(new ServerBootstrapper());

            _sampleConfig = TestHelper.GetSampleConfig();

            _getEmptyConfigResponse = Server.Get("application/empty");

            _setConfigResponse = Server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(_sampleConfig as object);
            });

            _getNewCreatedConfigResponse = Server.Get("application/new");

            _getNewCreatedConfigResult = _getNewCreatedConfigResponse.Body.AsJson();
        }

        [Test]
        public void getting_non_exist_config_should_be_empty()
        {
            _getEmptyConfigResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void setting_new_config_should_be_successfully()
        {
            _setConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_be_successfully()
        {
            _getNewCreatedConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_have_correct_values()
        {
            string application = _getNewCreatedConfigResult.application;
            string mongoUrl = _getNewCreatedConfigResult.persistence.mongo.url;

            application.Should().Be(_sampleConfig.Application);
            mongoUrl.Should().Be(_sampleConfig.Persistence.Mongo.Url);
        }
    }
}
