using DynamicConfigurator.Server.Configuration;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Module
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(ConfigurationManager configurationManager)
        {
            Get["/application/{application}"] = parameters =>
            {
                var application = (string)parameters.application;
                var environment = GetEnvironment();
                var data = configurationManager.Get(application, environment);

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

                configurationManager.Set(application, data, environment);

                return HttpStatusCode.OK;
            };
        }

        private string GetEnvironment()
        {
            return (string)Request.Query["environment"] ?? (string)Request.Query["env"];
        }
    }
}