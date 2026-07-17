using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Pipeline;
using Kyvara.Builder.Generators.Framework.Registry;

namespace Kyvara.Builder.Tests.GeneratorFramework;

public static class GeneratorPipelineTest
{
    public static async Task<bool> RunAsync()
    {
        var registry = new GeneratorRegistry();
        var writer = new MemoryGeneratedFileWriter();

        registry.Register(new CustomerGenerator());

        var pipeline = new GeneratorPipeline(
            registry,
            writer);

        var request = new GeneratorRequest
        {
            GeneratorName = "customer",
            OutputDirectory = Path.Combine(
                Path.GetTempPath(),
                "kyvara-pipeline-test"),
            OverwriteExistingFiles = true,
            Tokens = new Dictionary<string, string>
            {
                ["Namespace"] = "Sample.Domain",
                ["EntityName"] = "Customer"
            }
        };

        var result = await pipeline.ExecuteAsync(request);

        return result.Success &&
               result.Artifacts.Count == 1 &&
               writer.Files.Count == 1 &&
               writer.Files.ContainsKey(
                   "Domain/Entities/Customer.cs");
    }

    private sealed class CustomerGenerator
        : GeneratorBase
    {
        public override string Name => "customer";

        protected override Task GenerateCoreAsync(
            IGeneratorContext context,
            GeneratorResult result,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entityName = GetRequiredToken(
                context,
                "EntityName");

            var namespaceName = GetRequiredToken(
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

    private sealed class MemoryGeneratedFileWriter
        : IGeneratedFileWriter
    {
        public Dictionary<string, string> Files { get; } =
            new(StringComparer.OrdinalIgnoreCase);

        public Task WriteAsync(
            string outputDirectory,
            string relativePath,
            string content,
            bool overwrite,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var normalizedPath = relativePath.Replace(
                '\\',
                '/');

            if (!overwrite &&
                Files.ContainsKey(normalizedPath))
            {
                throw new IOException(
                    $"File '{normalizedPath}' already exists.");
            }

            Files[normalizedPath] = content;

            return Task.CompletedTask;
        }
    }
}
