using System;
using DynamicConfigurator.Server;
using Nancy.Hosting.Self;
using NUnit.Framework;

namespace DynamicConfigurator.Client.Test
{
    [SetUpFixture]
    public class ApiTestFixture
    {
        private NancyHost _configurationServerSelfHost;

        public static Uri ConfigurationServerUri = new Uri("http://localhost:36349");

        [OneTimeSetUp]
        public void SetUp()
        {
            _configurationServerSelfHost = DynamicConfiguratorServerSelfHost.Start(ConfigurationServerUri);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _configurationServerSelfHost.Dispose();
        }
    }
}
