using System.IO;
using Autofac;
using DynamicConfigurator.Server.Configuration;
using DynamicConfigurator.Server.Persistance;
using FluentAssertions;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Server.Test.When_storing_configs
{
    [TestFixture]
    public class with_file_storage
    {
        private static Browser _server;
        private dynamic sampleConfig;
        private bool isFileExist;
        private string createdFilePath;

        [SetUp]
        public void SetUp()
        {
            var applicationName = "new";
            var containerBuilder = new ServerContainerBuilder();

            containerBuilder.RegisterType<FileBasedConfigurationRepository>().AsImplementedInterfaces();

            _server = new Browser(new ServerBootstrapper(containerBuilder));

            sampleConfig = TestHelper.GetSampleConfig();

            _server.Post($"application/{applicationName}", context =>
            {
                context.HttpRequest();
                context.JsonBody(sampleConfig as object);
            });

            createdFilePath = Path.Combine(Directory.GetCurrentDirectory(), FileBasedConfigurationRepository.ConfigFolder, applicationName);

            isFileExist = File.Exists(createdFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(createdFilePath))
            {
                File.Delete(createdFilePath);
            }
        }

        [Test]
        public void config_file_should_be_created()
        {
            isFileExist.Should().BeTrue();
        }
    }
}
