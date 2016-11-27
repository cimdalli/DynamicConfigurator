using System;
using DynamicConfigurator.Server;
using DynamicConfigurator.Server.Api;
using Nancy.Hosting.Self;
using NUnit.Framework;

namespace DynamicConfigurator.Client.Test
{
    [SetUpFixture]
    public class ApiTestFixture
    {
        private NancyHost configurationServerSelfHost;

        public static readonly Uri ConfigurationServerUri = new Uri("http://localhost:36349");

        [OneTimeSetUp]
        public void SetUp()
        {
            configurationServerSelfHost = SelfHost.Start(ConfigurationServerUri);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            configurationServerSelfHost.Dispose();
        }
    }
}
