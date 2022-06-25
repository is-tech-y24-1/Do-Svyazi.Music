using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DS.Application.CQRS.Helpers;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<TRequest> _logger;
    
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<TRequest> logger)
    {
        _validators = validators;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation($"Validated!, {_validators.Count()}");
        if (!_validators.Any()) return await next();
        var context = new ValidationContext<TRequest>(request);
            
        var validationResults = await Task
            .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();
            
        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}