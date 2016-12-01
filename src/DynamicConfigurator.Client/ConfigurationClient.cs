using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace DynamicConfigurator.Client
{
    public class ConfigurationClient : IConfigurationClient
    {
        public event ConfigHasChangedEventHandler ConfigHasChanged;

        private readonly HttpClient httpClient;
        

        public ConfigurationClient(string configurationServerUrl)
            : this(new Uri(configurationServerUrl))
        {
        }

        public ConfigurationClient(Uri configurationServerUri)
        {
            httpClient = CreateHttpClient(configurationServerUri);
        }
        
        public bool IsSameHost(string host)
        {
            return httpClient.BaseAddress.Host == host;
        }

        public T GetConfiguration<T>(string application, string environment = null)
        {
            var uri = new Uri($"application/{application}", UriKind.Relative);

            if (environment != null)
            {
                uri.AddQuery("environment", environment);
            }

            var response = httpClient.GetAsync(uri).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content.ReadAsAsync<T>().Result;
            }

            throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        public void SetConfiguration(string application, object data, string environment = null)
        {
            var uri = new Uri($"application/{application}", UriKind.Relative);

            if (environment != null)
            {
                uri.AddQuery("environment", environment);
            }

            var response = httpClient.PostAsync(uri, data, new JsonMediaTypeFormatter()).Result;

            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(content);
            }
        }

        public void NotifyConfigHasChanged()
        {
            ConfigHasChanged?.Invoke();
        }

        private static HttpClient CreateHttpClient(Uri uri)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = uri
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
