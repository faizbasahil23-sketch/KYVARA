using Kyvara.Builder.Generators.Framework.Core;

namespace Kyvara.Builder.Generators.Framework.Pipeline.Steps;

public sealed class ExecuteGeneratorStep : IPipelineStep
{
    public string Name => "Execute generator";

    public PipelineStage Stage =>
        PipelineStage.ExecutingGenerator;

    public async Task ExecuteAsync(
        PipelineExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        context.MoveTo(Stage);

        var generator = context.Generator
            ?? throw new GeneratorException(
                "Pipeline generator has not been resolved.");

        var generatorContext = context.GeneratorContext
            ?? throw new GeneratorException(
                "Generator context has not been created.");

        var result = await generator.GenerateAsync(
            generatorContext,
            cancellationToken);

        context.SetGeneratorResult(result);

        if (!result.Success)
        {
            var errors = result.Errors.Count > 0
                ? string.Join(
                    Environment.NewLine,
                    result.Errors)
                : "Unknown generator error.";

            throw new GeneratorException(
                $"Generator '{generator.Name}' failed." +
                Environment.NewLine +
                errors);
        }
    }
}
