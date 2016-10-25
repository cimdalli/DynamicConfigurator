using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace DynamicConfigurator.Server.Test
{
    public class StubHost
    {
        private readonly INancyBootstrapper bootstrapper;
        private NancyHost nancyHost;
        public string BaseUrl { get; } = "http://localhost:9698";


        public StubHost(INancyBootstrapper bootstrapper)
        {
            this.bootstrapper = bootstrapper;
        }

        public StubHost() : this(new DefaultNancyBootstrapper())
        {
        }


        public void Start()
        {
            nancyHost = new NancyHost(bootstrapper, new HostConfiguration
            {
                UrlReservations = new UrlReservations
                {
                    CreateAutomatically = true
                },
            }, new Uri(BaseUrl));

            nancyHost.Start();
        }

        public void Stop()
        {
            nancyHost.Dispose();
        }
    }
}