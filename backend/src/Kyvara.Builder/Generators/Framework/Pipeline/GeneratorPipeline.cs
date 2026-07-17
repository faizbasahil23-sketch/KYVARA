using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Pipeline.Steps;

namespace Kyvara.Builder.Generators.Framework.Pipeline;

public sealed class GeneratorPipeline : IGeneratorPipeline
{
    private readonly IReadOnlyList<IPipelineStep> _steps;

    public GeneratorPipeline(
        IGeneratorRegistry registry,
        IGeneratedFileWriter fileWriter)
    {
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(fileWriter);

        _steps =
        [
            new ValidateRequestStep(),
            new ResolveGeneratorStep(registry),
            new CreateGeneratorContextStep(),
            new ExecuteGeneratorStep(),
            new WriteArtifactsStep(fileWriter)
        ];
    }

    public GeneratorPipeline(
        IEnumerable<IPipelineStep> steps)
    {
        ArgumentNullException.ThrowIfNull(steps);

        _steps = steps.ToArray();

        if (_steps.Count == 0)
        {
            throw new GeneratorException(
                "Generator pipeline requires at least one step.");
        }
    }

    public async Task<GeneratorResult> ExecuteAsync(
        GeneratorRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var executionContext =
            new PipelineExecutionContext(request);

        try
        {
            foreach (var step in _steps)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await step.ExecuteAsync(
                    executionContext,
                    cancellationToken);
            }

            executionContext.Complete(
                PipelineStage.Completed);

            var result = executionContext.GeneratorResult
                ?? new GeneratorResult();

            result.AddMessage(
                $"Pipeline completed in " +
                $"{executionContext.Duration?.TotalMilliseconds:F0} ms.");

            return result;
        }
        catch (OperationCanceledException)
        {
            executionContext.Complete(
                PipelineStage.Cancelled);

            var result = GetOrCreateResult(
                executionContext);

            result.AddError(
                "Generator pipeline was cancelled.");

            return result;
        }
        catch (GeneratorException exception)
        {
            executionContext.Complete(
                PipelineStage.Failed);

            var result = GetOrCreateResult(
                executionContext);

            result.AddError(exception.Message);

            return result;
        }
        catch (Exception exception)
        {
            executionContext.Complete(
                PipelineStage.Failed);

            var result = GetOrCreateResult(
                executionContext);

            result.AddError(
                $"Unexpected pipeline error: " +
                $"{exception.Message}");

            return result;
        }
    }

    private static GeneratorResult GetOrCreateResult(
        PipelineExecutionContext executionContext)
    {
        return executionContext.GeneratorResult
            ?? new GeneratorResult();
    }
}
