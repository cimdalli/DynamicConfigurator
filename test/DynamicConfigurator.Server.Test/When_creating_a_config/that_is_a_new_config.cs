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

        BrowserResponse getEmptyConfigResponse;
        BrowserResponse setConfigResponse;
        BrowserResponse getNewCreatedConfigResponse;
        private dynamic getNewCreatedConfigResult;
        private dynamic sampleConfig;

        [SetUp]
        public void SetUp()
        {
            Server = new Browser(new ServerBootstrapper());

            sampleConfig = TestHelper.GetSampleConfig();

            getEmptyConfigResponse = Server.Get("application/empty");

            setConfigResponse = Server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });

            getNewCreatedConfigResponse = Server.Get("application/new");

            getNewCreatedConfigResult = getNewCreatedConfigResponse.Body.AsJson();
        }

        [Test]
        public void getting_non_exist_config_should_be_empty()
        {
            getEmptyConfigResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void setting_new_config_should_be_successfully()
        {
            setConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_be_successfully()
        {
            getNewCreatedConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_have_correct_values()
        {
            string application = getNewCreatedConfigResult.application;
            string mongoUrl = getNewCreatedConfigResult.persistence.mongo.url;

            application.Should().Be(sampleConfig.Application);
            mongoUrl.Should().Be(sampleConfig.Persistence.Mongo.Url);
        }
    }
}
