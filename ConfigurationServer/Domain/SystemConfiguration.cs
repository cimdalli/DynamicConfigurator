using System.Collections.Generic;

namespace DynamicConfigurator.Server.Domain
{
    public class SystemConfiguration
    {
        public List<string> Clients { get; private set; }

        public SystemConfiguration()
        {
            Clients = new List<string>();
        }
    }
}
