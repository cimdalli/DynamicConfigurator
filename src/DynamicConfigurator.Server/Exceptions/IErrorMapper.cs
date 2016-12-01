using System;
using Nancy;

namespace DynamicConfigurator.Server.Api.Exceptions
{
    public interface IErrorMapper
    {
        HttpStatusCode GetStatusCode(Exception exception);
    }
}
