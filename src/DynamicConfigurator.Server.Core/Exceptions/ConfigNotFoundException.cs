using System;

namespace DynamicConfigurator.Server.Exceptions
{
    public class ConfigNotFoundException : Exception
    {
        public ConfigNotFoundException(string application, string environment)
            : base($"Config not found for application: {application} / environment: {environment}")
        {
        }
    }
}
