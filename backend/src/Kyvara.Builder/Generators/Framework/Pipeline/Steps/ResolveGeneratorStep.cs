using Kyvara.Builder.Generators.Framework.Abstractions;

namespace Kyvara.Builder.Generators.Framework.Pipeline.Steps;

public sealed class ResolveGeneratorStep : IPipelineStep
{
    private readonly IGeneratorRegistry _registry;

    public ResolveGeneratorStep(
        IGeneratorRegistry registry)
    {
        ArgumentNullException.ThrowIfNull(registry);
        _registry = registry;
    }

    public string Name => "Resolve generator";

    public PipelineStage Stage =>
        PipelineStage.ResolvingGenerator;

    public Task ExecuteAsync(
        PipelineExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        context.MoveTo(Stage);

        var generator = _registry.Resolve(
            context.Request.GeneratorName);

        context.SetGenerator(generator);

        return Task.CompletedTask;
    }
}
