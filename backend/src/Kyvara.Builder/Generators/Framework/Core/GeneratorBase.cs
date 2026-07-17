using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Core;

public abstract class GeneratorBase : IGenerator
{
    public abstract string Name { get; }

    public async Task<GeneratorResult> GenerateAsync(
        IGeneratorContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var result = new GeneratorResult();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            await ValidateContextAsync(
                context,
                cancellationToken);

            await GenerateCoreAsync(
                context,
                result,
                cancellationToken);

            if (result.Success)
            {
                result.AddMessage(
                    $"Generator '{Name}' completed successfully.");
            }
        }
        catch (OperationCanceledException)
        {
            result.AddError(
                $"Generator '{Name}' was cancelled.");
        }
        catch (GeneratorException exception)
        {
            result.AddError(exception.Message);
        }
        catch (Exception exception)
        {
            result.AddError(
                $"Generator '{Name}' failed: {exception.Message}");
        }

        return result;
    }

    protected virtual Task ValidateContextAsync(
        IGeneratorContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(context.OutputDirectory))
        {
            throw new GeneratorException(
                "Generator output directory is required.");
        }

        return Task.CompletedTask;
    }

    protected abstract Task GenerateCoreAsync(
        IGeneratorContext context,
        GeneratorResult result,
        CancellationToken cancellationToken);

    protected static void AddArtifact(
        GeneratorResult result,
        string relativePath,
        string content)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        ArgumentNullException.ThrowIfNull(content);

        result.AddArtifact(
            new GeneratorArtifact(
                NormalizeRelativePath(relativePath),
                content));
    }

    private static string NormalizeRelativePath(
        string relativePath)
    {
        var normalized = relativePath
            .Replace(
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar)
            .TrimStart(
                Path.DirectorySeparatorChar);

        if (Path.IsPathRooted(normalized))
        {
            throw new GeneratorException(
                "Generated artifact path must be relative.");
        }

        if (normalized
            .Split(Path.DirectorySeparatorChar)
            .Any(segment => segment == ".."))
        {
            throw new GeneratorException(
                "Generated artifact path cannot traverse parent directories.");
        }

        return normalized;
    }
}
