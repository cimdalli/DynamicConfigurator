using System.Collections.Generic;

namespace DynamicConfigurator.Common.Domain
{
    public class SystemConfiguration
    {
        public List<RegistrationInfo> RegistredClients { get; private set; }

        public SystemConfiguration()
        {
            RegistredClients = new List<RegistrationInfo>();
        }
    }
}
