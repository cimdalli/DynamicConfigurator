using System;
using System.Collections.Generic;
using DynamicConfigurator.Server.Api.Configuration;
using Nancy;

namespace DynamicConfigurator.Server.Api.Exceptions
{

    public interface IDictionaryBasedErrorMapper : IErrorMapper
    {
        void Map<T>(HttpStatusCode statusCode) where T : System.Exception;
    }


    public class DictionaryBasedErrorMapper : IDictionaryBasedErrorMapper
    {
        private readonly Dictionary<Type, HttpStatusCode> _lookup;

        public DictionaryBasedErrorMapper()
        {
            _lookup = new Dictionary<Type, HttpStatusCode>();
        }

        public void Map<T>(HttpStatusCode statusCode) where T : System.Exception
        {
            _lookup.Add(typeof(T), statusCode);
        }

        public HttpStatusCode GetStatusCode(System.Exception exception)
        {
            HttpStatusCode statusCode;

            return _lookup.TryGetValue(exception.GetType(), out statusCode) ? statusCode : HttpStatusCode.InternalServerError;
        }
    }
}