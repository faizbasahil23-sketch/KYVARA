using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.FileSystem;
using Kyvara.Builder.Generators.Framework.Pipeline;
using Kyvara.Builder.Generators.Framework.Registry;

namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Builds and configures the KYVARA generator framework.
/// </summary>
public sealed class GeneratorFrameworkBuilder
{
    private readonly List<IGenerator> _generators = [];

    private IGeneratedFileWriter _fileWriter =
        GeneratedFileWriterFactory.CreateDefault();

    /// <summary>
    /// Registers a generator with the framework.
    /// </summary>
    public GeneratorFrameworkBuilder AddGenerator(
        IGenerator generator)
    {
        ArgumentNullException.ThrowIfNull(generator);

        if (string.IsNullOrWhiteSpace(generator.Name))
        {
            throw new ArgumentException(
                "Generator name cannot be empty.",
                nameof(generator));
        }

        var alreadyRegistered = _generators.Any(
            existing => string.Equals(
                existing.Name,
                generator.Name,
                StringComparison.OrdinalIgnoreCase));

        if (alreadyRegistered)
        {
            throw new InvalidOperationException(
                $"Generator '{generator.Name}' has already been added.");
        }

        _generators.Add(generator);

        return this;
    }

    /// <summary>
    /// Registers multiple generators with the framework.
    /// </summary>
    public GeneratorFrameworkBuilder AddGenerators(
        IEnumerable<IGenerator> generators)
    {
        ArgumentNullException.ThrowIfNull(generators);

        foreach (var generator in generators)
        {
            AddGenerator(generator);
        }

        return this;
    }

    /// <summary>
    /// Uses a custom generated-file writer.
    /// </summary>
    public GeneratorFrameworkBuilder UseFileWriter(
        IGeneratedFileWriter fileWriter)
    {
        ArgumentNullException.ThrowIfNull(fileWriter);

        _fileWriter = fileWriter;

        return this;
    }

    /// <summary>
    /// Uses the default generated-file writer.
    /// </summary>
    public GeneratorFrameworkBuilder UseDefaultFileWriter()
    {
        _fileWriter =
            GeneratedFileWriterFactory.CreateDefault();

        return this;
    }

    /// <summary>
    /// Uses a writer that creates backups before overwriting.
    /// </summary>
    public GeneratorFrameworkBuilder UseSafeOverwriteFileWriter()
    {
        _fileWriter =
            GeneratedFileWriterFactory.CreateSafeOverwrite();

        return this;
    }

    /// <summary>
    /// Uses a dry-run writer that does not modify physical files.
    /// </summary>
    public GeneratorFrameworkBuilder UseDryRunFileWriter()
    {
        _fileWriter =
            GeneratedFileWriterFactory.CreateDryRun();

        return this;
    }

    /// <summary>
    /// Builds a registry containing all configured generators.
    /// </summary>
    public IGeneratorRegistry BuildRegistry()
    {
        var registry = new GeneratorRegistry();

        RegisterGenerators(registry);

        return registry;
    }

    /// <summary>
    /// Builds only the generator pipeline.
    /// </summary>
    public IGeneratorPipeline Build()
    {
        var registry = BuildRegistry();

        return new GeneratorPipeline(
            registry,
            _fileWriter);
    }

    /// <summary>
    /// Builds the complete generator framework runtime.
    /// </summary>
    public GeneratorFramework BuildFramework()
    {
        var registry = new GeneratorRegistry();

        RegisterGenerators(registry);

        var pipeline = new GeneratorPipeline(
            registry,
            _fileWriter);

        return new GeneratorFramework(
            registry,
            pipeline);
    }

    private void RegisterGenerators(
        IGeneratorRegistry registry)
    {
        ArgumentNullException.ThrowIfNull(registry);

        foreach (var generator in _generators)
        {
            registry.Register(generator);
        }
    }
}
