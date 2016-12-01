using Nancy;

namespace DynamicConfigurator.Client.Nancy
{
    public class ConfigurationModule : NancyModule
    {
        public ConfigurationModule(IConfigurationClient configurationClient) : base("config")
        {
            EnableAuthorization(configurationClient);

            Get["check"] = parameters => HttpStatusCode.OK;

            Post["notify"] = parameters =>
            {
                configurationClient.NotifyConfigHasChanged();

                return HttpStatusCode.OK;
            };
        }

        private void EnableAuthorization(IConfigurationClient configurationClient)
        {
            Before.AddItemToEndOfPipeline(context => 
                configurationClient.IsSameHost(context.Request.UserHostAddress) ? HttpStatusCode.Unauthorized : context.Response);
        }
    }
}
