using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Pipeline;

public sealed class PipelineExecutionContext
{
    public PipelineExecutionContext(GeneratorRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        Request = request;
        StartedAt = DateTimeOffset.UtcNow;
    }

    public GeneratorRequest Request { get; }

    public PipelineStage Stage { get; private set; } =
        PipelineStage.NotStarted;

    public DateTimeOffset StartedAt { get; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public IGenerator? Generator { get; private set; }

    public IGeneratorContext? GeneratorContext { get; private set; }

    public GeneratorResult? GeneratorResult { get; private set; }

    public TimeSpan? Duration =>
        CompletedAt.HasValue
            ? CompletedAt.Value - StartedAt
            : null;

    public void MoveTo(PipelineStage stage)
    {
        Stage = stage;
    }

    public void SetGenerator(IGenerator generator)
    {
        ArgumentNullException.ThrowIfNull(generator);
        Generator = generator;
    }

    public void SetGeneratorContext(
        IGeneratorContext generatorContext)
    {
        ArgumentNullException.ThrowIfNull(generatorContext);
        GeneratorContext = generatorContext;
    }

    public void SetGeneratorResult(
        GeneratorResult generatorResult)
    {
        ArgumentNullException.ThrowIfNull(generatorResult);
        GeneratorResult = generatorResult;
    }

    public void Complete(PipelineStage finalStage)
    {
        Stage = finalStage;
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
