using Nancy;

namespace DynamicConfigurator.Client.Nancy
{
    class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(IConfigurationClient configurationClient)
        {
            Before.AddItemToEndOfPipeline(context =>
            {
                if (configurationClient.ConfigurationServerUri.Host != context.Request.UserHostAddress)
                {
                    return HttpStatusCode.Unauthorized;
                }
                return context.Response;
            });

            Post["update"] = parameters =>
            {
                configurationClient.NotifyConfigHasChanged();

                return HttpStatusCode.OK;
            };
        }
    }
}
