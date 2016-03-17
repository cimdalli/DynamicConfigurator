using DynamicConfigurator.Server.Domain;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json;

namespace DynamicConfigurator.Server.Module
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(ConfigurationManager configurationManager)
        {
            var system = configurationManager.GetOrCreate("system", new SystemConfiguration());

            Get["/application/{application}"] = parameters =>
            {
                var application = (string)parameters.application;
                var data = configurationManager.Get(application);

                return JsonConvert.SerializeObject(data);
            };

            Post["/application/{application}"] = parameters =>
            {
                var application = (string)parameters.application;
                var content = Request.Body.AsString();
                var data = JsonConvert.DeserializeObject<object>(content);

                configurationManager.Add(application, data);

                return HttpStatusCode.OK;
            };

            Post["/register"] = ctx =>
            {
                var clientAddress = Request.UserHostAddress;
                var clients = system.Clients;

                if (!clients.Contains(clientAddress))
                {
                    clients.Add(clientAddress);
                }

                return HttpStatusCode.OK;

            };
        }
    }
}