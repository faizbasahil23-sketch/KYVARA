namespace Kyvara.Builder.Generators.Application;

public sealed class PipelineBehaviorGenerator
{
    public string Generate()
    {
        return """
using MediatR;

namespace {{Namespace}}.Behaviors;

public sealed class LoggingBehavior<TRequest,TResponse>
    : IPipelineBehavior<TRequest,TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // TODO:
        // Logging
        // Validation
        // Performance
        // Audit

        return await next();
    }
}
""";
    }
}
