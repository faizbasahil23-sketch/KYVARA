using Kyvara.Builder.Generators.Framework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kyvara.Builder.Generators.Domain.DependencyInjection;

/// <summary>
/// Provides dependency-injection registration for all built-in
/// KYVARA domain generators.
/// </summary>
public static class DomainGeneratorServiceCollectionExtensions
{
    /// <summary>
    /// Registers all built-in domain generators.
    /// </summary>
    public static IServiceCollection AddKyvaraDomainGenerators(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddKyvaraGenerator<global::Kyvara.Builder.Generators.Domain.Entities.DomainEntityGenerator>();

        services.AddKyvaraGenerator<global::Kyvara.Builder.Generators.Domain.Aggregates.DomainAggregateGenerator>();

        services.AddKyvaraGenerator<global::Kyvara.Builder.Generators.Domain.ValueObjects.DomainValueObjectGenerator>();

        services.AddKyvaraGenerator<global::Kyvara.Builder.Generators.Domain.Events.DomainEventGenerator>();

        services.AddKyvaraGenerator<global::Kyvara.Builder.Generators.Domain.Exceptions.DomainExceptionGenerator>();

        return services;
    }
}
