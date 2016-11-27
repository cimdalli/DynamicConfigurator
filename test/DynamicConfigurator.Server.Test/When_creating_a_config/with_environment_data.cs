using DynamicConfigurator.Server.Api.Configuration;
using DynamicConfigurator.Server.Configuration;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_creating_a_config
{
    [TestFixture]
    public class with_environment_data
    {
        private static Browser _server;

        private BrowserResponse setConfigResponse;
        private BrowserResponse setEnvironmentConfigResponse;
        private BrowserResponse getCreatedEnvironmentConfigResponse;
        private dynamic getEnvironmentConfigResult;
        private dynamic sampleConfig;
        private dynamic environmentConfig;

        [SetUp]
        public void SetUp()
        {
            _server = new Browser(new ServerBootstrapper());

            sampleConfig = TestHelper.GetSampleConfig();
            environmentConfig = TestHelper.GetEnvironmentOverrideConfig();

            setConfigResponse = _server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });

            setEnvironmentConfigResponse = _server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(environmentConfig as object);
                context.Query(TestHelper.Environment, "test");
            });

            getCreatedEnvironmentConfigResponse = _server.Get("application/new", context =>
            {
                context.Query(TestHelper.Environment, "test");
            });

            getEnvironmentConfigResult = getCreatedEnvironmentConfigResponse.Body.AsJson();
        }

        [Test]
        public void new_config_should_be_set_successfully()
        {
            setConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            setEnvironmentConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void created_config_should_be_gotten_successfully()
        {
            getCreatedEnvironmentConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void created_config_should_have_correct_values()
        {
            string application = getEnvironmentConfigResult.application;
            string mongoUrl = getEnvironmentConfigResult.persistence.mongo.url;

            application.Should().Be(sampleConfig.Application);
            mongoUrl.Should().Be(environmentConfig.Persistence.Mongo.Url);
        }
    }
}
