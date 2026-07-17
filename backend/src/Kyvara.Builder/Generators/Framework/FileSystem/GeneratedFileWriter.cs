using System.Text;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.FileSystem.Validation;

namespace Kyvara.Builder.Generators.Framework.FileSystem;

public sealed class GeneratedFileWriter
    : IGeneratedFileWriter
{
    private readonly GeneratedFileWriterOptions _options;
    private readonly IGeneratedFileWriteObserver? _observer;

    public GeneratedFileWriter(
        GeneratedFileWriterOptions? options = null,
        IGeneratedFileWriteObserver? observer = null)
    {
        _options = options ??
            new GeneratedFileWriterOptions();

        _observer = observer;

        ValidateOptions(_options);
    }

    public async Task WriteAsync(
        string outputDirectory,
        string relativePath,
        string content,
        bool overwrite,
        CancellationToken cancellationToken = default)
    {
        var result = await WriteWithResultAsync(
            outputDirectory,
            relativePath,
            content,
            overwrite,
            cancellationToken);

        if (!result.Success)
        {
            throw new GeneratedFileWriterException(
                result.Message ??
                $"Unable to write generated file '{relativePath}'.");
        }
    }

    public async Task<GeneratedFileWriteResult> WriteWithResultAsync(
        string outputDirectory,
        string relativePath,
        string content,
        bool overwrite,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        ArgumentNullException.ThrowIfNull(content);

        cancellationToken.ThrowIfCancellationRequested();

        var normalizedRelativePath =
            GeneratedFilePathGuard.NormalizeRelativePath(
                relativePath);

        string fullPath;

        try
        {
            fullPath = GeneratedFilePathGuard.ResolveSafePath(
                outputDirectory,
                relativePath);
        }
        catch (Exception exception)
        {
            var failedResult =
                new GeneratedFileWriteResult(
                    normalizedRelativePath,
                    string.Empty,
                    GeneratedFileWriteStatus.Failed,
                    false,
                    null,
                    exception.Message);

            Notify(failedResult);
            return failedResult;
        }

        try
        {
            var normalizedContent =
                GeneratedContentNormalizer.Normalize(
                    content,
                    _options.EnsureTrailingNewLine);

            var fileExists = File.Exists(fullPath);

            if (fileExists &&
                _options.SkipUnchangedFiles)
            {
                var currentContent =
                    await File.ReadAllTextAsync(
                        fullPath,
                        cancellationToken);

                var normalizedCurrentContent =
                    GeneratedContentNormalizer.Normalize(
                        currentContent,
                        _options.EnsureTrailingNewLine);

                if (string.Equals(
                        normalizedCurrentContent,
                        normalizedContent,
                        StringComparison.Ordinal))
                {
                    var skippedResult =
                        new GeneratedFileWriteResult(
                            normalizedRelativePath,
                            fullPath,
                            GeneratedFileWriteStatus.Skipped,
                            false,
                            null,
                            "Existing file is unchanged.");

                    Notify(skippedResult);
                    return skippedResult;
                }
            }

            if (fileExists && !overwrite)
            {
                var failedResult =
                    new GeneratedFileWriteResult(
                        normalizedRelativePath,
                        fullPath,
                        GeneratedFileWriteStatus.Failed,
                        false,
                        null,
                        $"File '{normalizedRelativePath}' already exists and overwrite is disabled.");

                Notify(failedResult);
                return failedResult;
            }

            if (_options.DryRun)
            {
                var dryRunResult =
                    new GeneratedFileWriteResult(
                        normalizedRelativePath,
                        fullPath,
                        GeneratedFileWriteStatus.DryRun,
                        false,
                        null,
                        fileExists
                            ? "Dry run: existing file would be overwritten."
                            : "Dry run: new file would be created.");

                Notify(dryRunResult);
                return dryRunResult;
            }

            var outputRoot = Path.GetFullPath(
                outputDirectory);

            if (!Directory.Exists(outputRoot))
            {
                if (!_options.CreateOutputDirectory)
                {
                    var failedResult =
                        new GeneratedFileWriteResult(
                            normalizedRelativePath,
                            fullPath,
                            GeneratedFileWriteStatus.Failed,
                            false,
                            null,
                            $"Output directory '{outputRoot}' does not exist.");

                    Notify(failedResult);
                    return failedResult;
                }

                Directory.CreateDirectory(outputRoot);
            }

            var targetDirectory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrWhiteSpace(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            string? backupPath = null;
            var backupCreated = false;

            if (fileExists &&
                overwrite &&
                _options.CreateBackupBeforeOverwrite)
            {
                backupPath = CreateUniqueBackupPath(fullPath);

                File.Copy(
                    fullPath,
                    backupPath,
                    overwrite: false);

                backupCreated = true;
            }

            await AtomicWriteAsync(
                fullPath,
                normalizedContent,
                cancellationToken);

            var result =
                new GeneratedFileWriteResult(
                    normalizedRelativePath,
                    fullPath,
                    fileExists
                        ? GeneratedFileWriteStatus.Overwritten
                        : GeneratedFileWriteStatus.Created,
                    backupCreated,
                    backupPath,
                    fileExists
                        ? "File overwritten successfully."
                        : "File created successfully.");

            Notify(result);
            return result;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            var failedResult =
                new GeneratedFileWriteResult(
                    normalizedRelativePath,
                    fullPath,
                    GeneratedFileWriteStatus.Failed,
                    false,
                    null,
                    exception.Message);

            Notify(failedResult);
            return failedResult;
        }
    }

    private static async Task AtomicWriteAsync(
        string fullPath,
        string content,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(fullPath)
            ?? throw new GeneratedFileWriterException(
                $"Unable to resolve directory for '{fullPath}'.");

        var temporaryPath = Path.Combine(
            directory,
            $".{Path.GetFileName(fullPath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            await File.WriteAllTextAsync(
                temporaryPath,
                content,
                new UTF8Encoding(
                    encoderShouldEmitUTF8Identifier: false),
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            File.Move(
                temporaryPath,
                fullPath,
                overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private string CreateUniqueBackupPath(
        string fullPath)
    {
        var extension = _options.BackupExtension;

        var baseBackupPath = fullPath + extension;

        if (!File.Exists(baseBackupPath))
        {
            return baseBackupPath;
        }

        var timestamp =
            DateTimeOffset.UtcNow.ToString(
                "yyyyMMddHHmmssfff");

        return fullPath +
               "." +
               timestamp +
               extension;
    }

    private void Notify(
        GeneratedFileWriteResult result)
    {
        _observer?.OnCompleted(result);
    }

    private static void ValidateOptions(
        GeneratedFileWriterOptions options)
    {
        if (string.IsNullOrWhiteSpace(
                options.BackupExtension))
        {
            throw new ArgumentException(
                "Backup extension cannot be empty.",
                nameof(options));
        }

        if (options.BackupExtension.IndexOfAny(
                Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException(
                "Backup extension contains invalid characters.",
                nameof(options));
        }
    }
}
