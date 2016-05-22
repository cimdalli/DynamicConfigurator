using System.Collections.Specialized;

namespace DynamicConfigurator.Server.Configuration
{
    public class TypeDescriptor
    {
        public string Type { get; set; }
        public NameValueCollection Args { get; set; }

        public TypeDescriptor(string type, NameValueCollection args)
        {
            Type = type;
            Args = args;
        }
    }
}
