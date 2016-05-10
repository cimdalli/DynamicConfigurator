using DynamicConfigurator.Server.Configuration;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_creating_a_config
{
    [TestFixture]
    public class that_is_with_environment_data
    {
        public static Browser Server;

        BrowserResponse setConfigResponse;
        BrowserResponse setEnvironmentConfigResponse;
        BrowserResponse getCreatedEnvironmentConfigResponse;
        private dynamic getEnvironmentConfigResult;
        private dynamic sampleConfig;
        private dynamic environmentConfig;

        [SetUp]
        public void SetUp()
        {
            Server = new Browser(new ServerBootstrapper());

            sampleConfig = TestHelper.GetSampleConfig();
            environmentConfig = TestHelper.GetEnvironmentOverrideConfig();

            setConfigResponse = Server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });

            setEnvironmentConfigResponse = Server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(environmentConfig as object);
                context.Query(TestHelper.Environment, "test");
            });

            getCreatedEnvironmentConfigResponse = Server.Get("application/new", context =>
            {
                context.Query(TestHelper.Environment, "test");
            });

            getEnvironmentConfigResult = getCreatedEnvironmentConfigResponse.Body.AsJson();
        }

        [Test]
        public void setting_new_config_should_be_successfully()
        {
            setConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            setEnvironmentConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_be_successfully()
        {
            getCreatedEnvironmentConfigResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void getting_new_created_config_should_have_correct_values()
        {
            string application = getEnvironmentConfigResult.application;
            string mongoUrl = getEnvironmentConfigResult.persistence.mongo.url;

            application.Should().Be(sampleConfig.Application);
            mongoUrl.Should().Be(environmentConfig.Persistence.Mongo.Url);
        }
    }
}
