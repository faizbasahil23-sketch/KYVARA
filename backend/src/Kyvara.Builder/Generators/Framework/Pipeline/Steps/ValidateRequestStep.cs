using Kyvara.Builder.Generators.Framework.Validation;

namespace Kyvara.Builder.Generators.Framework.Pipeline.Steps;

public sealed class ValidateRequestStep : IPipelineStep
{
    public string Name => "Validate generator request";

    public PipelineStage Stage =>
        PipelineStage.ValidatingRequest;

    public Task ExecuteAsync(
        PipelineExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        context.MoveTo(Stage);

        GeneratorRequestValidator.Validate(context.Request);

        return Task.CompletedTask;
    }
}
