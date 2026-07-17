namespace Kyvara.Builder.Generators.Framework.Pipeline.Steps;

public interface IPipelineStep
{
    string Name { get; }

    PipelineStage Stage { get; }

    Task ExecuteAsync(
        PipelineExecutionContext context,
        CancellationToken cancellationToken = default);
}
