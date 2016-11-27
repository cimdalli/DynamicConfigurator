using Nancy;

namespace DynamicConfigurator.Server.Api.Exceptions
{
    public interface IErrorMapper
    {
        HttpStatusCode GetStatusCode(System.Exception exception);
    }
}
