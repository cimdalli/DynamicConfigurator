﻿using DynamicConfigurator.Server.Exceptions;
using Nancy;
using Nancy.Responses;

namespace DynamicConfigurator.Server.Api.Exceptions
{
    public class ExceptionResponse : JsonResponse
    {
        public System.Exception Exception { get; set; }

        public ExceptionResponse(System.Exception exception, HttpStatusCode statusCode)
            : base(new ExceptionModel(exception), new DefaultJsonSerializer())
        {
            Exception = exception;
            StatusCode = statusCode;
        }
    }
}
