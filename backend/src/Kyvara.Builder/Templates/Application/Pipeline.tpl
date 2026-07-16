namespace Kyvara.Builder.Generators.Application;

public sealed class PipelineGenerator
{
    public string Generate()
    {
        return """
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace {{Namespace}}.Pipeline;

public static class PipelineRegistration
{
    public static IServiceCollection AddGeneratedPipeline(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<PipelineRegistration>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));

        return services;
    }
}

public sealed class ValidationBehavior<TRequest,TResponse>
    : IPipelineBehavior<TRequest,TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await next();
    }
}

public sealed class LoggingBehavior<TRequest,TResponse>
    : IPipelineBehavior<TRequest,TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest,TResponse>> _logger;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest,TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(typeof(TRequest).Name);

        return await next();
    }
}

public sealed class PerformanceBehavior<TRequest,TResponse>
    : IPipelineBehavior<TRequest,TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var response = await next();

        sw.Stop();

        return response;
    }
}

public sealed class ExceptionBehavior<TRequest,TResponse>
    : IPipelineBehavior<TRequest,TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch
        {
            throw;
        }
    }
}
""";
    }
}
