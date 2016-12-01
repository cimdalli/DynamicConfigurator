using System;

namespace DynamicConfigurator.Server.Exceptions
{
    public class ExceptionModel
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public ExceptionModel(Exception exception)
        {
            Message = exception.Message;
            Type = exception.GetType().Name;
        }
    }
}