using Newtonsoft.Json;

namespace DynamicConfigurator.Server.Configuration
{
    public sealed class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
