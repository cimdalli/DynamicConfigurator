using DynamicConfigurator.Server.Exceptions;
using DynamicConfigurator.Server.Services;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Api.Module
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(ConfigurationService configurationService)
        {
            Get["/all"] = parameters =>
            {
                return configurationService.GetConfigKeys();
            };

            Get["/application/{application}"] = parameters =>
            {
                var application = (string)parameters.application;
                var environment = GetEnvironment();
                var client = GetClient();
                var data = configurationService.GetConfig(application, environment, client);

                if (data != null)
                {
                    return Response.AsJson(data);
                }

                throw new ConfigNotFoundException(application, environment);
            };

            Post["/application/{application}"] = parameters =>
            {
                var application = (string)parameters.application;
                var environment = GetEnvironment();
                var content = Request.Body.AsString();
                var data = JObject.Parse(content);

                configurationService.SetConfig(application, data, environment);

                return HttpStatusCode.OK;
            };
        }

        private string GetEnvironment()
        {
            return (string)Request.Query["environment"] ?? (string)Request.Query["env"];
        }

        private string GetClient()
        {
            return (string)Request.Query["client"];
        }
    }
}