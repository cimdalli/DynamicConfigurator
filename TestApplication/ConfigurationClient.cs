using System;
using System.Configuration.Abstractions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DynamicConfigurator.Client
{
    public interface IConfigurationClient
    {
        T GetConfiguration<T>(string application, string environment = null);

        dynamic GetConfiguration(string application, string environment = null);

        void SetConfiguration(string application, object data, string environment = null);
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

            var response = _httpClient.GetAsync(path).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }

        public object GetConfiguration(string application, string environment = null)
        {
            return GetConfiguration<object>(application, environment);
        }

        public void SetConfiguration(string application, object data, string environment = null)
        {
            var path = $"application/{application}";
            if (environment != null)
            {
                path += $"/environment/{environment}";
            }

            var response = _httpClient.PostAsync(path, data, new JsonMediaTypeFormatter()).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(content);
            }
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
            return httpClient.PostAsJsonAsync("register", new
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
