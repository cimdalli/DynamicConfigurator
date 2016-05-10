using System;

namespace DynamicConfigurator.Common.Configuration
{
    public class ExceptionModel
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }

        public ExceptionModel(Exception exception)
        {
            Message = exception.Message;
            Source = exception.Source;
            Type = exception.GetType().FullName;
        }
    }
}