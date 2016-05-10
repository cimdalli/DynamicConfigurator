using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using DynamicConfigurator.Common;
using DynamicConfigurator.Common.Domain;
using Newtonsoft.Json;

namespace DynamicConfigurator.Client
{
    public delegate void ConfigHasChangedEventHandler();

    public class ConfigurationClient : IConfigurationClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public Uri ConfigurationServerUri => _httpClient.BaseAddress;
        public event ConfigHasChangedEventHandler ConfigHasChanged;


        public ConfigurationClient(string configurationServerUrl)
            : this(new Uri(configurationServerUrl)) { }
        

        public ConfigurationClient(Uri configurationServerUri)
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            _httpClient = CreateHttpClient(configurationServerUri);

            //_httpClient.PostAsJsonAsync("register", (string)null);
        }


        public T GetConfiguration<T>(string application, string environment = null) where T : class, IConfigData, new()
        {
            var uri = new Uri($"application/{application}", UriKind.Relative);
            if (environment != null)
            {
                uri.AddQuery("environment", environment);
            }

            var response = _httpClient.GetAsync(uri).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<T>(content, _jsonSerializerSettings);

            return data;
        }

        //public object GetConfiguration(string application, string environment = null)
        //{
        //    return GetConfiguration<object>(application, environment);
        //}

        public void SetConfiguration(string application, object data, string environment = null)
        {
            var uri = new Uri($"application/{application}", UriKind.Relative);
            if (environment != null)
            {
                uri.AddQuery("environment", environment);
            }

            var response = _httpClient.PostAsync(uri, data, new JsonMediaTypeFormatter()).Result;
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
