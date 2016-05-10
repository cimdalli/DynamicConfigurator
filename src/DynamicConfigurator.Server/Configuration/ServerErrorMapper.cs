using DynamicConfigurator.Common.Configuration;
using Nancy;

namespace DynamicConfigurator.Server.Configuration
{
    public class ServerErrorMapper : DictionaryBasedErrorMapper
    {
        public ServerErrorMapper()
        {
            Map<ConfigNotFoundException>(HttpStatusCode.NotFound);
        }
    }
}
