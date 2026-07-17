namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Defines the runtime configuration used to initialize the
/// KYVARA Generator Framework.
/// </summary>
public sealed class GeneratorFrameworkOptions
{
    /// <summary>
    /// Gets or sets the root directory containing generator templates.
    /// </summary>
    public string TemplateRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the root directory where generated artifacts are written.
    /// </summary>
    public string OutputRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether existing files may be overwritten.
    /// </summary>
    public bool OverwriteExistingFiles { get; set; }

    /// <summary>
    /// Gets or sets whether unchanged files should be skipped.
    /// </summary>
    public bool SkipUnchangedFiles { get; set; } = true;

    /// <summary>
    /// Gets or sets whether file writes should use atomic replacement.
    /// </summary>
    public bool UseAtomicWrites { get; set; } = true;

    /// <summary>
    /// Gets or sets whether backup files should be created before overwrite.
    /// </summary>
    public bool CreateBackups { get; set; }

    /// <summary>
    /// Gets or sets whether the framework operates without changing files.
    /// </summary>
    public bool DryRun { get; set; }

    /// <summary>
    /// Gets or sets whether file templates should be cached in memory.
    /// </summary>
    public bool EnableTemplateCache { get; set; } = true;

    /// <summary>
    /// Gets or sets whether unresolved template placeholders cause failure.
    /// </summary>
    public bool StrictTemplateValidation { get; set; } = true;

    /// <summary>
    /// Gets or sets whether generator names are matched case-sensitively.
    /// </summary>
    public bool CaseSensitiveGeneratorNames { get; set; }

    /// <summary>
    /// Creates a detached copy of the current configuration.
    /// </summary>
    public GeneratorFrameworkOptions Clone()
    {
        return new GeneratorFrameworkOptions
        {
            TemplateRoot = TemplateRoot,
            OutputRoot = OutputRoot,
            OverwriteExistingFiles = OverwriteExistingFiles,
            SkipUnchangedFiles = SkipUnchangedFiles,
            UseAtomicWrites = UseAtomicWrites,
            CreateBackups = CreateBackups,
            DryRun = DryRun,
            EnableTemplateCache = EnableTemplateCache,
            StrictTemplateValidation = StrictTemplateValidation,
            CaseSensitiveGeneratorNames = CaseSensitiveGeneratorNames
        };
    }
}
