namespace Kyvara.Builder.Generators.Framework.Pipeline;

public enum PipelineStage
{
    NotStarted = 0,
    ValidatingRequest = 1,
    ResolvingGenerator = 2,
    CreatingContext = 3,
    ExecutingGenerator = 4,
    WritingArtifacts = 5,
    Completed = 6,
    Failed = 7,
    Cancelled = 8
}
