using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface IGeneratorPipeline
{
    Task<GeneratorResult> ExecuteAsync(
        GeneratorRequest request,
        CancellationToken cancellationToken = default);
}
