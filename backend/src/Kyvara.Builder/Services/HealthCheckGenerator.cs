namespace Kyvara.Builder.Services;

public sealed class HealthCheckGenerator
{
    public string Generate()
    {
        return
"""
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Generated.Health;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddGeneratedHealthChecks(
        this IServiceCollection services)
    {
        services.AddHealthChecks();

        return services;
    }

    public static WebApplication MapGeneratedHealthChecks(
        this WebApplication app)
    {
        app.MapHealthChecks("/health");

        app.MapHealthChecks("/ready");

        app.MapHealthChecks("/live");

        return app;
    }
}
""";
    }
}
