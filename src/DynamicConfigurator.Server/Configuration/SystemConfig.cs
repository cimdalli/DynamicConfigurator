using System.Collections.Generic;

namespace DynamicConfigurator.Server.Configuration
{
    public class SystemConfig
    {
        public Dictionary<string, List<string>> RegisteredClients { get; private set; }

        public SystemConfig()
        {
            RegisteredClients = new Dictionary<string, List<string>>();
        }
    }
}
