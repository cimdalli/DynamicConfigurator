using DynamicConfigurator.Server.Configuration;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Module
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(ConfigurationService configurationService)
        {
            Get["/application/{application}"] = parameters =>
            {
                var address = Request.UserHostAddress;
                //var host = Request.UserHostName;
                var application = (string)parameters.application;
                var environment = GetEnvironment();
                var data = configurationService.Get(application, environment);

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

                configurationService.Set(application, data, environment);

                return HttpStatusCode.OK;
            };
        }

        private string GetEnvironment()
        {
            return (string)Request.Query["environment"] ?? (string)Request.Query["env"];
        }
    }
}