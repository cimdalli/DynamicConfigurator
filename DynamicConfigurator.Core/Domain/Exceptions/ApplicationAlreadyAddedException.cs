using System;

namespace ConfigurationServer.Domain.Exceptions
{
    class ApplicationAlreadyAddedException : Exception
    {
        public ApplicationAlreadyAddedException() : base("Application already added.")
        {
        }
    }
}
