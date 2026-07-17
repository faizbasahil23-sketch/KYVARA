namespace Kyvara.Builder.Generators.Framework.FileSystem;

public static class GeneratedFileWriterFactory
{
    public static GeneratedFileWriter CreateDefault(
        IGeneratedFileWriteObserver? observer = null)
    {
        return new GeneratedFileWriter(
            new GeneratedFileWriterOptions
            {
                DryRun = false,
                CreateBackupBeforeOverwrite = false,
                SkipUnchangedFiles = true,
                EnsureTrailingNewLine = true,
                CreateOutputDirectory = true
            },
            observer);
    }

    public static GeneratedFileWriter CreateSafeOverwrite(
        IGeneratedFileWriteObserver? observer = null)
    {
        return new GeneratedFileWriter(
            new GeneratedFileWriterOptions
            {
                DryRun = false,
                CreateBackupBeforeOverwrite = true,
                BackupExtension = ".bak",
                SkipUnchangedFiles = true,
                EnsureTrailingNewLine = true,
                CreateOutputDirectory = true
            },
            observer);
    }

    public static GeneratedFileWriter CreateDryRun(
        IGeneratedFileWriteObserver? observer = null)
    {
        return new GeneratedFileWriter(
            new GeneratedFileWriterOptions
            {
                DryRun = true,
                CreateBackupBeforeOverwrite = false,
                SkipUnchangedFiles = true,
                EnsureTrailingNewLine = true,
                CreateOutputDirectory = false
            },
            observer);
    }
}
