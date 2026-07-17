using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface IGenerator
{
    string Name { get; }

    Task<GeneratorResult> GenerateAsync(
        IGeneratorContext context,
        CancellationToken cancellationToken = default);
}
