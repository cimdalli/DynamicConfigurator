using System;
using Nancy;
using Nancy.Hosting.Self;

namespace DynamicConfigurator.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            HostConfiguration hostConfigs = new HostConfiguration

            {
                UrlReservations = { CreateAutomatically = true }
            };

            using (var host = new NancyHost(new Uri("http://localhost:1234"), new DefaultNancyBootstrapper(), hostConfigs))
            {
                host.Start();
                Console.ReadLine();
            }
        }
    }
}
