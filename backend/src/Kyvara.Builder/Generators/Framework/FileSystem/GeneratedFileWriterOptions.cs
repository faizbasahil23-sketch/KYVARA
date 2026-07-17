namespace Kyvara.Builder.Generators.Framework.FileSystem;

public sealed class GeneratedFileWriterOptions
{
    public bool DryRun { get; init; }

    public bool CreateBackupBeforeOverwrite { get; init; }

    public string BackupExtension { get; init; } = ".bak";

    public bool SkipUnchangedFiles { get; init; } = true;

    public bool EnsureTrailingNewLine { get; init; } = true;

    public bool CreateOutputDirectory { get; init; } = true;
}
