using DynamicConfigurator.Common.Domain;

namespace DynamicConfigurator.Server.Configuration
{
    public class ConfigNotFoundException : FormattedException
    {
        public ConfigNotFoundException(string application, string environment)
            : base("Config not found for application: {{{0}}} / environment: {{{1}}}", application, environment)
        {
        }
    }
}
