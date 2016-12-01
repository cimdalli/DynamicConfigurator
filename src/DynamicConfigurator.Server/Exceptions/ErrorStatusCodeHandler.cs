using Nancy;
using Nancy.ErrorHandling;

namespace DynamicConfigurator.Server.Api.Exceptions
{
    public class ErrorStatusCodeHandler : IStatusCodeHandler
    {

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound
                || statusCode == HttpStatusCode.InternalServerError
                || statusCode == HttpStatusCode.Forbidden
                || statusCode == HttpStatusCode.Unauthorized;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            if (context.Response is ExceptionResponse) return;

            context.Response = new Response
            {
                StatusCode = statusCode
            };
        }
    }
}
