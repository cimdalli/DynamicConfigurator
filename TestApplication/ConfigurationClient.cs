using System;
using System.Configuration.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ClientApplication
{
    public interface IConfigurationClient
    {
        T GetConfiguration<T>(string application, string environment = null);

        object GetConfiguration(string application, string environment = null);
    }

    public class ConfigurationClient : IConfigurationClient
    {
        HttpClient _httpClient;

        private ConfigurationClient()
        {
            var config = ConfigurationManager.Instance.AppSettings["ConfigurationServer"];
            if (!string.IsNullOrEmpty(config))
            {
                RegisterConfigurationClient(config);
            }
        }

        public ConfigurationClient(string configurationServerUrl)
        {
            RegisterConfigurationClient(configurationServerUrl);
        }

        public ConfigurationClient(Uri configurationServerUri)
        {
            RegisterConfigurationClient(configurationServerUri);
        }


        public static IConfigurationClient Instance => new ConfigurationClient();

        public T GetConfiguration<T>(string application, string environment = null)
        {
            var path = $"application/{application}";
            if (environment != null)
            {
                path += $"/environment/{environment}";
            }
            return _httpClient.GetAsync(path).Result.Content.ReadAsAsync<T>().Result;
        }

        public object GetConfiguration(string application, string environment = null)
        {
            return GetConfiguration<object>(application, environment);
        }



        private bool RegisterConfigurationClient(string configurationServerUrl)
        {
            try
            {
                var configurationServerUri = new Uri(configurationServerUrl);
                return RegisterConfigurationClient(configurationServerUri);
            }
            catch (UriFormatException)
            {
                throw new Exception($"Invalid URI: {configurationServerUrl}");
            }
        }

        private bool RegisterConfigurationClient(Uri configurationServerUri)
        {
            var httpClient = CreateHttpClient(configurationServerUri);
            return RegisterConfigurationClient(httpClient);
        }

        private bool RegisterConfigurationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            return httpClient
                .PostAsJsonAsync("register", new
                {
                    IP = "1.1.1.1",
                    MachineName = "machine"
                }).Result.IsSuccessStatusCode;
        }

        private HttpClient CreateHttpClient(Uri uri)
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
