using MediatR;
using Microsoft.Extensions.Logging;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace DS.Application.CQRS.Helpers;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>  where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        string requestName = request.GetType().Name;
        string requestGuid = Guid.NewGuid().ToString();
        string requestNameWithGuid = $"{requestName} [{requestGuid}]";

        _logger.LogInformation($"[START] {requestNameWithGuid}");
        TResponse response;

        try
        {
            response = await next();
        }
        catch (Exception e)
        {
            _logger.LogInformation($"[ERROR] {e.Message} {requestNameWithGuid}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"[END] {requestNameWithGuid}");
        }

        return response;
    }
}