using DynamicConfigurator.Server.Exceptions;
using Nancy;

namespace DynamicConfigurator.Server.Api.Exceptions
{
    public class ServerErrorMapper : DictionaryBasedErrorMapper
    {
        public ServerErrorMapper()
        {
            Map<ConfigNotFoundException>(HttpStatusCode.NotFound);
        }
    }
}
