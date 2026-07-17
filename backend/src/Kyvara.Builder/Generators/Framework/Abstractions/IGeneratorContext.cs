namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface IGeneratorContext
{
    string OutputDirectory { get; }

    IReadOnlyDictionary<string, string> Tokens { get; }

    bool OverwriteExistingFiles { get; }
}
