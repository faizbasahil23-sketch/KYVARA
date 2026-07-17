namespace Kyvara.Builder.Generators.Framework.FileSystem;

public sealed record GeneratedFileWriteResult(
    string RelativePath,
    string FullPath,
    GeneratedFileWriteStatus Status,
    bool BackupCreated,
    string? BackupPath,
    string? Message)
{
    public bool Success =>
        Status is not GeneratedFileWriteStatus.Failed;
}
