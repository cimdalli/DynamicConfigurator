using Nancy;

namespace DynamicConfigurator.Server.Exceptions
{
    public class ServerErrorMapper : DictionaryBasedErrorMapper
    {
        public ServerErrorMapper()
        {
            Map<ConfigNotFoundException>(HttpStatusCode.NotFound);
        }
    }
}
