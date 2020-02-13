using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace ProspaAspNetCoreApiNsb.Infrastructure.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger = Log.ForContext<LoggingBehavior<TRequest, TResponse>>();

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var traceId = Guid.NewGuid();

            try
            {
                _logger.Verbose("Handling {TRequest}. TraceId: {TraceId} {UtcNow}", typeof(TRequest).FullName, traceId, DateTime.UtcNow);

                var response = await next();

                _logger.Verbose("Handled {TResponse}. TraceId: {TraceId} {UtcNow}", typeof(TResponse).FullName, traceId, DateTime.UtcNow);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while handling {TRequest}. TraceId: {TraceId} {UtcNow}", typeof(TRequest).FullName, traceId, DateTime.UtcNow);

                throw;
            }
        }
    }
}
