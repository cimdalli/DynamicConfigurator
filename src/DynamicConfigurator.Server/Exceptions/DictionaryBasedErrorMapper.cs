using System;
using System.Collections.Generic;
using Nancy;

namespace DynamicConfigurator.Server.Api.Exceptions
{

    public interface IDictionaryBasedErrorMapper : IErrorMapper
    {
        void Map<T>(HttpStatusCode statusCode) where T : Exception;
    }


    public class DictionaryBasedErrorMapper : IDictionaryBasedErrorMapper
    {
        private readonly Dictionary<Type, HttpStatusCode> lookup;

        public DictionaryBasedErrorMapper()
        {
            lookup = new Dictionary<Type, HttpStatusCode>();
        }

        public void Map<T>(HttpStatusCode statusCode) where T : Exception
        {
            lookup.Add(typeof(T), statusCode);
        }

        public HttpStatusCode GetStatusCode(System.Exception exception)
        {
            HttpStatusCode statusCode;

            return lookup.TryGetValue(exception.GetType(), out statusCode) ? statusCode : HttpStatusCode.InternalServerError;
        }
    }
}