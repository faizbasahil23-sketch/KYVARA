namespace Kyvara.Builder.Generators.Infrastructure;

public sealed class InfrastructureDependencyInjectionGenerator
{
    public string Generate()
    {
        return """
using Microsoft.Extensions.DependencyInjection;

namespace {{Namespace}}.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(
            typeof(IRepository<>),
            typeof(Repository<>));

        return services;
    }
}
""";
    }
}
