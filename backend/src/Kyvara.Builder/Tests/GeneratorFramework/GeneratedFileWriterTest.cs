using Kyvara.Builder.Generators.Framework.FileSystem;

namespace Kyvara.Builder.Tests.GeneratorFramework;

public static class GeneratedFileWriterTest
{
    public static async Task<bool> RunAsync()
    {
        var root = CreateTemporaryDirectory();

        try
        {
            var journal =
                new GeneratedFileWriteJournal();

            var writer =
                GeneratedFileWriterFactory.CreateSafeOverwrite(
                    journal);

            var created =
                await writer.WriteWithResultAsync(
                    root,
                    "Domain/Entities/Customer.cs",
                    "public sealed class Customer { }",
                    overwrite: false);

            var overwritten =
                await writer.WriteWithResultAsync(
                    root,
                    "Domain/Entities/Customer.cs",
                    "public sealed class Customer { public Guid Id { get; } }",
                    overwrite: true);

            var skipped =
                await writer.WriteWithResultAsync(
                    root,
                    "Domain/Entities/Customer.cs",
                    "public sealed class Customer { public Guid Id { get; } }",
                    overwrite: true);

            return
                created.Status ==
                    GeneratedFileWriteStatus.Created &&
                overwritten.Status ==
                    GeneratedFileWriteStatus.Overwritten &&
                overwritten.BackupCreated &&
                skipped.Status ==
                    GeneratedFileWriteStatus.Skipped &&
                File.Exists(created.FullPath) &&
                overwritten.BackupPath is not null &&
                File.Exists(overwritten.BackupPath) &&
                journal.Results.Count == 3;
        }
        finally
        {
            DeleteDirectory(root);
        }
    }

    public static async Task<bool> RejectsOverwriteAsync()
    {
        var root = CreateTemporaryDirectory();

        try
        {
            var writer =
                GeneratedFileWriterFactory.CreateDefault();

            await writer.WriteAsync(
                root,
                "Sample.cs",
                "version one",
                overwrite: false);

            var result =
                await writer.WriteWithResultAsync(
                    root,
                    "Sample.cs",
                    "version two",
                    overwrite: false);

            return
                result.Status ==
                    GeneratedFileWriteStatus.Failed &&
                File.ReadAllText(
                    Path.Combine(root, "Sample.cs"))
                    .Contains(
                        "version one",
                        StringComparison.Ordinal);
        }
        finally
        {
            DeleteDirectory(root);
        }
    }

    public static async Task<bool> SupportsDryRunAsync()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            "kyvara-file-writer-dry-run",
            Guid.NewGuid().ToString("N"));

        var journal =
            new GeneratedFileWriteJournal();

        var writer =
            GeneratedFileWriterFactory.CreateDryRun(
                journal);

        var result =
            await writer.WriteWithResultAsync(
                root,
                "Application/CustomerService.cs",
                "public sealed class CustomerService { }",
                overwrite: false);

        return
            result.Status ==
                GeneratedFileWriteStatus.DryRun &&
            !File.Exists(result.FullPath) &&
            !Directory.Exists(root) &&
            journal.DryRunCount == 1;
    }

    public static async Task<bool> RejectsPathTraversalAsync()
    {
        var root = CreateTemporaryDirectory();

        try
        {
            var writer =
                GeneratedFileWriterFactory.CreateDefault();

            var result =
                await writer.WriteWithResultAsync(
                    root,
                    "../Secret.cs",
                    "secret",
                    overwrite: false);

            return
                result.Status ==
                    GeneratedFileWriteStatus.Failed;
        }
        finally
        {
            DeleteDirectory(root);
        }
    }

    private static string CreateTemporaryDirectory()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            "kyvara-generated-file-writer-test",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);

        return root;
    }

    private static void DeleteDirectory(
        string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(
                path,
                recursive: true);
        }
    }
}
