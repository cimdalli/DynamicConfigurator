namespace DynamicConfigurator.Server.Exceptions
{
    public class ExceptionModel
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }

        public ExceptionModel(System.Exception exception)
        {
            Message = exception.Message;
            Source = exception.Source;
            Type = exception.GetType().FullName;
        }
    }
}