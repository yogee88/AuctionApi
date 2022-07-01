using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.ServiceHandlers.Exceptions
{
    public class UnhandledExceptionsHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>  where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionsHandler(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            try
            {
                return await next();
            }
            catch (Exception ex )
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "{@requestName}", requestName);
                throw;
            }
        }
    }
}
