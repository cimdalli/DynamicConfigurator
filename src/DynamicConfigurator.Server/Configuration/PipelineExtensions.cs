using System.Linq;
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

        public static void EnableCORS(this Nancy.Bootstrapper.IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (ctx.Request.Headers.Keys.Contains("Origin"))
                {
                    var origins = "" + string.Join(" ", ctx.Request.Headers["Origin"]);
                    ctx.Response.Headers["Access-Control-Allow-Origin"] = origins;

                    if (ctx.Request.Method == "OPTIONS")
                    {
                        // handle CORS preflight request
                        ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";

                        if (ctx.Request.Headers.Keys.Contains("Access-Control-Request-Headers"))
                        {
                            var allowedHeaders = "" + string.Join(", ", ctx.Request.Headers["Access-Control-Request-Headers"]);
                            ctx.Response.Headers["Access-Control-Allow-Headers"] = allowedHeaders;
                        }
                    }
                }
            });
        }
    }
}
