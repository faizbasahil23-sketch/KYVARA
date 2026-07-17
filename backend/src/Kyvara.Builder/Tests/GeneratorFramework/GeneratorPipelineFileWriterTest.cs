using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.FileSystem;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Pipeline;
using Kyvara.Builder.Generators.Framework.Registry;

namespace Kyvara.Builder.Tests.GeneratorFramework;

public static class GeneratorPipelineFileWriterTest
{
    public static async Task<bool> RunAsync()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            "kyvara-pipeline-file-writer-test",
            Guid.NewGuid().ToString("N"));

        try
        {
            var registry =
                new GeneratorRegistry();

            registry.Register(
                new SampleEntityGenerator());

            var journal =
                new GeneratedFileWriteJournal();

            var writer =
                GeneratedFileWriterFactory.CreateDefault(
                    journal);

            var pipeline =
                new GeneratorPipeline(
                    registry,
                    writer);

            var request =
                new GeneratorRequest
                {
                    GeneratorName = "sample-entity",
                    OutputDirectory = root,
                    OverwriteExistingFiles = false,
                    Tokens =
                        new Dictionary<string, string>(
                            StringComparer.OrdinalIgnoreCase)
                        {
                            ["EntityName"] = "Customer",
                            ["Namespace"] = "Sample.Domain"
                        }
                };

            var result =
                await pipeline.ExecuteAsync(request);

            var expectedFile = Path.Combine(
                root,
                "Domain",
                "Entities",
                "Customer.cs");

            return
                result.Success &&
                File.Exists(expectedFile) &&
                journal.CreatedCount == 1 &&
                File.ReadAllText(expectedFile)
                    .Contains(
                        "class Customer",
                        StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(
                    root,
                    recursive: true);
            }
        }
    }

    private sealed class SampleEntityGenerator
        : GeneratorBase
    {
        public override string Name =>
            "sample-entity";

        protected override Task GenerateCoreAsync(
            IGeneratorContext context,
            GeneratorResult result,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entityName =
                GetRequiredToken(
                    context,
                    "EntityName");

            var namespaceName =
                GetRequiredToken(
                    context,
                    "Namespace");

            var content = $$"""
                namespace {{namespaceName}}.Entities;

                public sealed class {{entityName}}
                {
                    public Guid Id { get; private set; }
                }
                """;

            AddArtifact(
                result,
                $"Domain/Entities/{entityName}.cs",
                content);

            return Task.CompletedTask;
        }

        private static string GetRequiredToken(
            IGeneratorContext context,
            string tokenName)
        {
            if (!context.Tokens.TryGetValue(
                    tokenName,
                    out var value) ||
                string.IsNullOrWhiteSpace(value))
            {
                throw new GeneratorException(
                    $"Required token '{tokenName}' is missing.");
            }

            return value;
        }
    }
}
