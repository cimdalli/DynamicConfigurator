using Common.Logging;
using DynamicConfigurator.Server.Api.Exceptions;
using Nancy.Bootstrapper;

namespace DynamicConfigurator.Server.Api.Configuration
{
    public static class PipelineExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger("ExceptionResponse");

        public static void EnableJsonErrorResponse(this IPipelines pipelines, IErrorMapper errorMapper)
        {
            pipelines.OnError += (context, exception) =>
            {
                Logger.Error("[Logger]", exception);

                var statusCode = errorMapper.GetStatusCode(exception);
                
                return new ExceptionResponse(exception, statusCode);
            };
        }
    }
}
