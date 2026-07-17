using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Registry;
using Kyvara.Builder.Generators.Framework.Validation;

namespace Kyvara.Builder.Tests;

public static class GeneratorFrameworkCoreTest
{
    public static async Task<bool> RunAsync()
    {
        var request = new GeneratorRequest
        {
            GeneratorName = "sample",
            OutputDirectory = Path.Combine(
                Path.GetTempPath(),
                "kyvara-generator-test"),
            Tokens = new Dictionary<string, string>
            {
                ["Name"] = "Customer"
            }
        };

        GeneratorRequestValidator.Validate(request);

        var context = new GeneratorContext(request);
        var registry = new GeneratorRegistry();

        registry.Register(new SampleGenerator());

        var generator = registry.Resolve("sample");

        var result = await generator.GenerateAsync(context);

        return result.Success &&
               result.Artifacts.Count == 1 &&
               registry.Contains("sample");
    }

    private sealed class SampleGenerator : GeneratorBase
    {
        public override string Name => "sample";

        protected override Task GenerateCoreAsync(
            IGeneratorContext context,
            GeneratorResult result,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            AddArtifact(
                result,
                "Sample.txt",
                "Generator framework is ready.");

            return Task.CompletedTask;
        }
    }
}
