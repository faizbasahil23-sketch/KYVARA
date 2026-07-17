using Kyvara.Builder.Generators.Framework.Core;

namespace Kyvara.Builder.Generators.Framework.Pipeline.Steps;

public sealed class CreateGeneratorContextStep : IPipelineStep
{
    public string Name => "Create generator context";

    public PipelineStage Stage =>
        PipelineStage.CreatingContext;

    public Task ExecuteAsync(
        PipelineExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        context.MoveTo(Stage);

        var generatorContext =
            new GeneratorContext(context.Request);

        context.SetGeneratorContext(generatorContext);

        return Task.CompletedTask;
    }
}
