using System;
using System.Net.Http;
using Common.Logging;

namespace DynamicConfigurator.Server.Notification
{
    public class HttpClientNotifier : IClientNotifier
    {
        private static readonly ILog Logger = LogManager.GetLogger<HttpClientNotifier>();

        public void NotifyClient(string client)
        {
            try
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(client)
                };
                httpClient.PostAsync("config/notify", null);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}