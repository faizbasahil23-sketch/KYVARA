using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;

namespace Kyvara.Builder.Generators.Framework.Registry;

public sealed class GeneratorRegistry : IGeneratorRegistry
{
    private readonly Dictionary<string, IGenerator> _generators =
        new(StringComparer.OrdinalIgnoreCase);

    public void Register(IGenerator generator)
    {
        ArgumentNullException.ThrowIfNull(generator);

        if (string.IsNullOrWhiteSpace(generator.Name))
        {
            throw new GeneratorException(
                "Generator name cannot be empty.");
        }

        if (!_generators.TryAdd(generator.Name, generator))
        {
            throw new GeneratorException(
                $"Generator '{generator.Name}' is already registered.");
        }
    }

    public bool Contains(string generatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(generatorName);
        return _generators.ContainsKey(generatorName);
    }

    public IGenerator Resolve(string generatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(generatorName);

        if (!_generators.TryGetValue(
            generatorName,
            out var generator))
        {
            throw new GeneratorException(
                $"Generator '{generatorName}' is not registered.");
        }

        return generator;
    }

    public IReadOnlyCollection<string> GetRegisteredNames()
    {
        return _generators.Keys
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
