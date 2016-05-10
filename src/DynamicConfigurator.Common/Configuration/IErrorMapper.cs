using System;
using Nancy;

namespace DynamicConfigurator.Common.Configuration
{
    public interface IErrorMapper
    {
        HttpStatusCode GetStatusCode(Exception exception);
    }
}
