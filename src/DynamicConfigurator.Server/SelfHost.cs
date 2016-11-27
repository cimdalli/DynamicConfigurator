using System;
using Common.Logging;
using DynamicConfigurator.Server.Api.Configuration;
using DynamicConfigurator.Server.Configuration;
using Nancy.Hosting.Self;

namespace DynamicConfigurator.Server.Api
{
    public class SelfHost
    {
        private static readonly ILog Logger = LogManager.GetLogger<SelfHost>();

        private static void Main(string[] args)
        {
            var selfHost = Start();
            Console.ReadLine();
            selfHost.Dispose();
        }

        private static NancyHost Start()
        {
            var port = ServerSettings.Current().App.ServerPort;

            return Start($"http://localhost:{port}");
        }

        private static NancyHost Start(string baseUrl)
        {
            return Start(new Uri(baseUrl));
        }

        public static NancyHost Start(Uri baseUri)
        {
            var hostConfigs = new HostConfiguration { UrlReservations = { CreateAutomatically = true } };
            var host = new NancyHost(baseUri, new ServerBootstrapper(), hostConfigs);

            Logger.Info("[DynamicConfigurator.Server] is been starting...");
            host.Start();
            Logger.Info("[DynamicConfigurator.Server] has started.");

            return host;
        }
    }
}
