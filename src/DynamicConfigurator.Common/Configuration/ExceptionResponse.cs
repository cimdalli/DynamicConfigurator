using System;
using Nancy;
using Nancy.Responses;

namespace DynamicConfigurator.Common.Configuration
{
    public class ExceptionResponse : JsonResponse
    {
        public Exception Exception { get; set; }

        public ExceptionResponse(Exception exception, HttpStatusCode statusCode)
            : base(new ExceptionModel(exception), new DefaultJsonSerializer())
        {
            Exception = exception;
            StatusCode = statusCode;
        }
    }
}
