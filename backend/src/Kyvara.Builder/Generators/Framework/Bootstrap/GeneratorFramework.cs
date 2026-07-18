using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Represents a fully configured generator framework runtime.
/// </summary>
public sealed class GeneratorFramework
{
    public GeneratorFramework(
        IGeneratorRegistry registry,
        IGeneratorPipeline pipeline)
    {
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(pipeline);

        Registry = registry;
        Pipeline = pipeline;
    }

    /// <summary>
    /// Gets the generator registry used by this runtime.
    /// </summary>
    public IGeneratorRegistry Registry { get; }

    /// <summary>
    /// Gets the generator execution pipeline.
    /// </summary>
    public IGeneratorPipeline Pipeline { get; }

    /// <summary>
    /// Gets the names of all registered generators.
    /// </summary>
    public IReadOnlyCollection<string> RegisteredGeneratorNames =>
        Registry.GetRegisteredNames();

    /// <summary>
    /// Determines whether a generator is registered.
    /// </summary>
    public bool ContainsGenerator(
        string generatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(generatorName);

        return Registry.Contains(generatorName);
    }

    /// <summary>
    /// Resolves a registered generator.
    /// </summary>
    public IGenerator ResolveGenerator(
        string generatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(generatorName);

        return Registry.Resolve(generatorName);
    }

    /// <summary>
    /// Executes a generator request through the configured pipeline.
    /// </summary>
    public Task<GeneratorResult> ExecuteAsync(
        GeneratorRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Pipeline.ExecuteAsync(
            request,
            cancellationToken);
    }
}
