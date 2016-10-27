using Nancy;
using Nancy.ModelBinding;

namespace DynamicConfigurator.Client.Nancy
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(IConfigurationClient configurationClient) : base("config")
        {
            Before.AddItemToEndOfPipeline(context =>
            {
                if (configurationClient.ConfigurationServerUri.Host != context.Request.UserHostAddress)
                {
                    return HttpStatusCode.Unauthorized;
                }
                return context.Response;
            });

            Get["check"] = parameters => HttpStatusCode.OK;

            Post["notify"] = parameters =>
            {
                var config = this.Bind<string>();

                configurationClient.NotifyConfigHasChanged(config);

                return HttpStatusCode.OK;
            };
        }
    }
}
