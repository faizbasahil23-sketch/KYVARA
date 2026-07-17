namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Represents a validated and normalized framework configuration.
/// </summary>
public sealed class GeneratorFrameworkConfiguration
{
    public GeneratorFrameworkConfiguration(GeneratorFrameworkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        TemplateRoot = Path.GetFullPath(options.TemplateRoot);
        OutputRoot = Path.GetFullPath(options.OutputRoot);
        OverwriteExistingFiles = options.OverwriteExistingFiles;
        SkipUnchangedFiles = options.SkipUnchangedFiles;
        UseAtomicWrites = options.UseAtomicWrites;
        CreateBackups = options.CreateBackups;
        DryRun = options.DryRun;
        EnableTemplateCache = options.EnableTemplateCache;
        StrictTemplateValidation = options.StrictTemplateValidation;
        CaseSensitiveGeneratorNames = options.CaseSensitiveGeneratorNames;
    }

    public string TemplateRoot { get; }

    public string OutputRoot { get; }

    public bool OverwriteExistingFiles { get; }

    public bool SkipUnchangedFiles { get; }

    public bool UseAtomicWrites { get; }

    public bool CreateBackups { get; }

    public bool DryRun { get; }

    public bool EnableTemplateCache { get; }

    public bool StrictTemplateValidation { get; }

    public bool CaseSensitiveGeneratorNames { get; }
}
