using System;

namespace DynamicConfigurator.Server.Domain.Exceptions
{
    class ApplicationAlreadyAddedException : Exception
    {
        public ApplicationAlreadyAddedException() : base("Application already added.")
        {
        }
    }
}
