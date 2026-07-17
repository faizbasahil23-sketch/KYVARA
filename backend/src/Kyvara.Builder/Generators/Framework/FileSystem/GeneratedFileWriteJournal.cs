namespace Kyvara.Builder.Generators.Framework.FileSystem;

public sealed class GeneratedFileWriteJournal
    : IGeneratedFileWriteObserver
{
    private readonly List<GeneratedFileWriteResult> _results = [];

    public IReadOnlyList<GeneratedFileWriteResult> Results =>
        _results;

    public int CreatedCount =>
        _results.Count(result =>
            result.Status == GeneratedFileWriteStatus.Created);

    public int OverwrittenCount =>
        _results.Count(result =>
            result.Status == GeneratedFileWriteStatus.Overwritten);

    public int SkippedCount =>
        _results.Count(result =>
            result.Status == GeneratedFileWriteStatus.Skipped);

    public int DryRunCount =>
        _results.Count(result =>
            result.Status == GeneratedFileWriteStatus.DryRun);

    public int FailedCount =>
        _results.Count(result =>
            result.Status == GeneratedFileWriteStatus.Failed);

    public void OnCompleted(
        GeneratedFileWriteResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        _results.Add(result);
    }

    public void Clear()
    {
        _results.Clear();
    }
}
