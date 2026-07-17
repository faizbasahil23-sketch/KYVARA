namespace Kyvara.Builder.Generators.Framework.Models;

public sealed class GeneratorRequest
{
    public required string GeneratorName { get; init; }

    public required string OutputDirectory { get; init; }

    public Dictionary<string, string> Tokens { get; init; } =
        new(StringComparer.OrdinalIgnoreCase);

    public bool OverwriteExistingFiles { get; init; }
}
