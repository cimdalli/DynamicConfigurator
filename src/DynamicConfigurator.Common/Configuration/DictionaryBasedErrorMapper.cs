using System;
using System.Collections.Generic;
using Nancy;

namespace DynamicConfigurator.Common.Configuration
{

    public interface IDictionaryBasedErrorMapper : IErrorMapper
    {
        void Map<T>(HttpStatusCode statusCode) where T : Exception;
    }


    public class DictionaryBasedErrorMapper : IDictionaryBasedErrorMapper
    {
        private readonly Dictionary<Type, HttpStatusCode> _lookup;

        public DictionaryBasedErrorMapper()
        {
            _lookup = new Dictionary<Type, HttpStatusCode>();
        }

        public void Map<T>(HttpStatusCode statusCode) where T : Exception
        {
            _lookup.Add(typeof(T), statusCode);
        }

        public HttpStatusCode GetStatusCode(Exception exception)
        {
            HttpStatusCode statusCode;

            return _lookup.TryGetValue(exception.GetType(), out statusCode) ? statusCode : HttpStatusCode.InternalServerError;
        }
    }
}