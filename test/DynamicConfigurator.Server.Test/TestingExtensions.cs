using Nancy.ModelBinding.DefaultBodyDeserializers;
using Nancy.Testing;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Test
{
    public static class TestingExtensions
    {
        public static dynamic AsJson(this BrowserResponseBodyWrapper bodyWrapper)
        {
            var response = bodyWrapper.Deserialize<string>(new JsonBodyDeserializer());
            return JObject.Parse(response);
        }
    }
}
