using System.Threading;
using DynamicConfigurator.Server.Configuration;
using FluentAssertions;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_registering_a_client
{
    [TestFixture]
    public class with_environment_data
    {
        private static Browser _server;
        private StubHost stubHost;

        private BrowserResponse setConfigResponse;
        private BrowserResponse setEnvironmentConfigResponse;
        private BrowserResponse getCreatedEnvironmentConfigResponse;
        private dynamic getEnvironmentConfigResult;
        private dynamic sampleConfig;
        private dynamic environmentConfig;
        private bool isConfigChanged;

        [SetUp]
        public void SetUp()
        {
            SetupStub();

            _server = new Browser(new ServerBootstrapper());

            sampleConfig = TestHelper.GetSampleConfig();
            environmentConfig = TestHelper.GetEnvironmentOverrideConfig();

            StoreBaseConfig();
            StoreOverloadConfig();
            GetConfigAndRegister();
            ChangeConfig();
            GetChangedConfig();

            //hack to get notify request
            Thread.Sleep(1000);
        }

        private void GetChangedConfig()
        {
            getEnvironmentConfigResult = getCreatedEnvironmentConfigResponse.Body.AsJson();
        }

        private void ChangeConfig()
        {
            setEnvironmentConfigResponse = _server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(environmentConfig as object);
                context.Query(TestHelper.Environment, "test");
            });
        }

        private void GetConfigAndRegister()
        {
            getCreatedEnvironmentConfigResponse = _server.Get("application/new", context =>
            {
                context.Query(TestHelper.Environment, "test");
                context.Query(TestHelper.Client, stubHost.BaseUrl);
            });
        }

        private void StoreOverloadConfig()
        {
            _server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(environmentConfig as object);
                context.Query(TestHelper.Environment, "test");
            });
        }

        private void StoreBaseConfig()
        {
            setConfigResponse = _server.Post("application/new", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });
        }

        private void SetupStub()
        {
            stubHost = new StubHost(
                new ConfigurableBootstrapper(with =>
                {
                    with.Module(new ConfigurableNancyModule(module =>
                    {
                        module.Post("config/notify", (o, nancyModule) =>
                        {
                            isConfigChanged = true;
                            return HttpStatusCode.OK;
                        });
                    }));
                }));

            stubHost.Start();
        }

        [TearDown]
        public void TearDown()
        {
            stubHost.Stop();
        }

        [Test]
        public void notify_client_when_config_has_been_changed()
        {
            isConfigChanged.Should().BeTrue();
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
