using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Core;

public sealed class GeneratorContext : IGeneratorContext
{
    private readonly IReadOnlyDictionary<string, string> _tokens;

    public GeneratorContext(GeneratorRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        OutputDirectory = Path.GetFullPath(request.OutputDirectory);
        OverwriteExistingFiles = request.OverwriteExistingFiles;

        _tokens = new Dictionary<string, string>(
            request.Tokens,
            StringComparer.OrdinalIgnoreCase);
    }

    public string OutputDirectory { get; }

    public IReadOnlyDictionary<string, string> Tokens => _tokens;

    public bool OverwriteExistingFiles { get; }

    public string GetRequiredToken(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (!_tokens.TryGetValue(name, out var value) ||
            string.IsNullOrWhiteSpace(value))
        {
            throw new GeneratorException(
                $"Required generator token '{name}' was not provided.");
        }

        return value;
    }

    public string? GetOptionalToken(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return _tokens.TryGetValue(name, out var value)
            ? value
            : null;
    }

    public bool HasToken(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _tokens.ContainsKey(name);
    }
}
