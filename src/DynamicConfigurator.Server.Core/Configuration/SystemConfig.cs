using System.Collections.Generic;

namespace DynamicConfigurator.Server.Configuration
{
    public class SystemConfig
    {
        public SortedDictionary<string, List<string>> RegisteredClients { get; private set; }

        public SystemConfig()
        {
            RegisteredClients = new SortedDictionary<string, List<string>>();
        }
    }
}
