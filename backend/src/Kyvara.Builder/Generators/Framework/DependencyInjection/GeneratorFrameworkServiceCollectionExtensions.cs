using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Bootstrap;
using Kyvara.Builder.Generators.Framework.FileSystem;
using Kyvara.Builder.Generators.Framework.Pipeline;
using Kyvara.Builder.Generators.Framework.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kyvara.Builder.Generators.Framework.DependencyInjection;

/// <summary>
/// Provides dependency-injection registration methods for the
/// KYVARA generator framework.
/// </summary>
public static class GeneratorFrameworkServiceCollectionExtensions
{
    /// <summary>
    /// Registers the generator framework with the default file writer.
    /// </summary>
    public static IServiceCollection AddKyvaraGeneratorFramework(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IGeneratedFileWriter>(
            _ => GeneratedFileWriterFactory.CreateDefault());

        RegisterFrameworkServices(services);

        return services;
    }

    /// <summary>
    /// Registers the generator framework with a custom file writer.
    /// </summary>
    public static IServiceCollection AddKyvaraGeneratorFramework(
        this IServiceCollection services,
        IGeneratedFileWriter fileWriter)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(fileWriter);

        services.RemoveAll<IGeneratedFileWriter>();
        services.AddSingleton(fileWriter);

        RegisterFrameworkServices(services);

        return services;
    }

    /// <summary>
    /// Registers the generator framework using a file-writer factory.
    /// </summary>
    public static IServiceCollection AddKyvaraGeneratorFramework(
        this IServiceCollection services,
        Func<IServiceProvider, IGeneratedFileWriter> fileWriterFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(fileWriterFactory);

        services.RemoveAll<IGeneratedFileWriter>();
        services.AddSingleton(fileWriterFactory);

        RegisterFrameworkServices(services);

        return services;
    }

    /// <summary>
    /// Registers a generator implementation as a singleton.
    /// </summary>
    public static IServiceCollection AddKyvaraGenerator<TGenerator>(
        this IServiceCollection services)
        where TGenerator : class, IGenerator
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IGenerator, TGenerator>());

        return services;
    }

    /// <summary>
    /// Registers a generator instance.
    /// </summary>
    public static IServiceCollection AddKyvaraGenerator(
        this IServiceCollection services,
        IGenerator generator)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(generator);

        services.AddSingleton(generator);
        services.AddSingleton<IGenerator>(
            serviceProvider =>
                serviceProvider.GetRequiredService(
                    generator.GetType()) as IGenerator
                ?? throw new InvalidOperationException(
                    $"Unable to resolve generator '{generator.Name}'."));

        return services;
    }

    /// <summary>
    /// Registers a generator using a factory.
    /// </summary>
    public static IServiceCollection AddKyvaraGenerator<TGenerator>(
        this IServiceCollection services,
        Func<IServiceProvider, TGenerator> generatorFactory)
        where TGenerator : class, IGenerator
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(generatorFactory);

        services.AddSingleton<TGenerator>(generatorFactory);

        services.AddSingleton<IGenerator>(
            serviceProvider =>
                serviceProvider.GetRequiredService<TGenerator>());

        return services;
    }

    /// <summary>
    /// Uses the safe-overwrite generated-file writer.
    /// </summary>
    public static IServiceCollection UseKyvaraSafeOverwriteWriter(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.RemoveAll<IGeneratedFileWriter>();

        services.AddSingleton<IGeneratedFileWriter>(
            _ => GeneratedFileWriterFactory.CreateSafeOverwrite());

        return services;
    }

    /// <summary>
    /// Uses the dry-run generated-file writer.
    /// </summary>
    public static IServiceCollection UseKyvaraDryRunWriter(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.RemoveAll<IGeneratedFileWriter>();

        services.AddSingleton<IGeneratedFileWriter>(
            _ => GeneratedFileWriterFactory.CreateDryRun());

        return services;
    }

    private static void RegisterFrameworkServices(
        IServiceCollection services)
    {
        services.TryAddSingleton<IGeneratorRegistry>(
            serviceProvider =>
            {
                var registry = new GeneratorRegistry();

                var generators =
                    serviceProvider.GetServices<IGenerator>();

                foreach (var generator in generators)
                {
                    registry.Register(generator);
                }

                return registry;
            });

        services.TryAddSingleton<IGeneratorPipeline>(
            serviceProvider =>
                new GeneratorPipeline(
                    serviceProvider.GetRequiredService<
                        IGeneratorRegistry>(),
                    serviceProvider.GetRequiredService<
                        IGeneratedFileWriter>()));

        services.TryAddSingleton<GeneratorFramework>(
            serviceProvider =>
                new GeneratorFramework(
                    serviceProvider.GetRequiredService<
                        IGeneratorRegistry>(),
                    serviceProvider.GetRequiredService<
                        IGeneratorPipeline>()));
    }
}
