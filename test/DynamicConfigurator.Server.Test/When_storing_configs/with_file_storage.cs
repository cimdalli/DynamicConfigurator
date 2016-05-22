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
        public static Browser Server;
        private dynamic _sampleConfig;
        private bool _isFileExist;
        private string _createdFilePath;

        [SetUp]
        public void SetUp()
        {
            var applicationName = "new";
            var containerBuilder = new ServerContainerBuilder();

            containerBuilder.RegisterType<FileBasedConfigurationRepository>().AsImplementedInterfaces();

            Server = new Browser(new ServerBootstrapper(containerBuilder));

            _sampleConfig = TestHelper.GetSampleConfig();

            Server.Post($"application/{applicationName}", context =>
            {
                context.HttpRequest();
                context.JsonBody(_sampleConfig as object);
            });

            _createdFilePath = Path.Combine(Directory.GetCurrentDirectory(), FileBasedConfigurationRepository.ConfigFolder, applicationName);

            _isFileExist = File.Exists(_createdFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_createdFilePath))
            {
                File.Delete(_createdFilePath);
            }
        }

        [Test]
        public void config_file_should_be_created()
        {
            _isFileExist.Should().BeTrue();
        }
    }
}
