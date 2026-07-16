namespace Kyvara.Builder.Services;

public sealed class DependencyInjectionGenerator
{
    public string Generate()
    {
        return
"""
using Microsoft.Extensions.DependencyInjection;

namespace Generated;

public static class DependencyInjection
{
    public static IServiceCollection AddGeneratedServices(
        this IServiceCollection services)
    {
        services.AddApplication();

        services.AddInfrastructure();

        services.AddRepositories();

        services.AddPersistence();

        services.AddOpenApi();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddAuthorization();

        services.AddAuthentication();

        services.AddHealthChecks();

        return services;
    }
}
""";
    }
}
