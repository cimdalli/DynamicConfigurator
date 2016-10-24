using Nancy;

namespace DynamicConfigurator.Server.Exceptions
{
    public interface IErrorMapper
    {
        HttpStatusCode GetStatusCode(System.Exception exception);
    }
}
