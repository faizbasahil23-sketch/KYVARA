using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;

namespace Kyvara.Builder.Generators.Framework.Pipeline.Steps;

public sealed class WriteArtifactsStep : IPipelineStep
{
    private readonly IGeneratedFileWriter _fileWriter;

    public WriteArtifactsStep(
        IGeneratedFileWriter fileWriter)
    {
        ArgumentNullException.ThrowIfNull(fileWriter);
        _fileWriter = fileWriter;
    }

    public string Name => "Write generated artifacts";

    public PipelineStage Stage =>
        PipelineStage.WritingArtifacts;

    public async Task ExecuteAsync(
        PipelineExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        context.MoveTo(Stage);

        var result = context.GeneratorResult
            ?? throw new GeneratorException(
                "Generator result is not available.");

        foreach (var artifact in result.Artifacts)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _fileWriter.WriteAsync(
                context.Request.OutputDirectory,
                artifact.RelativePath,
                artifact.Content,
                context.Request.OverwriteExistingFiles,
                cancellationToken);
        }

        result.AddMessage(
            $"{result.Artifacts.Count} artifact(s) written.");
    }
}
