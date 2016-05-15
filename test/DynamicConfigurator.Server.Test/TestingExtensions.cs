using Nancy.Testing;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurator.Server.Test
{
    public static class TestingExtensions
    {
        public static dynamic AsJson(this BrowserResponseBodyWrapper bodyWrapper)
        {
            var json = bodyWrapper.AsString();
            return JObject.Parse(json);
        }
    }
}
