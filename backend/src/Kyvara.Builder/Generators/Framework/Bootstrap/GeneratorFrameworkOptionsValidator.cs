namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Validates and normalizes Generator Framework bootstrap options.
/// </summary>
public static class GeneratorFrameworkOptionsValidator
{
    public static GeneratorFrameworkConfiguration ValidateAndCreate(
        GeneratorFrameworkOptions options,
        string? baseDirectory = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        var normalizedOptions = options.Clone();
        var effectiveBaseDirectory = string.IsNullOrWhiteSpace(baseDirectory)
            ? Directory.GetCurrentDirectory()
            : Path.GetFullPath(baseDirectory);

        DefaultTemplateConfiguration.Apply(
            normalizedOptions,
            effectiveBaseDirectory);

        DefaultFileWriterConfiguration.Apply(
            normalizedOptions,
            effectiveBaseDirectory);

        ValidateDirectoryPath(
            normalizedOptions.TemplateRoot,
            nameof(normalizedOptions.TemplateRoot));

        ValidateDirectoryPath(
            normalizedOptions.OutputRoot,
            nameof(normalizedOptions.OutputRoot));

        if (normalizedOptions.CreateBackups &&
            !normalizedOptions.OverwriteExistingFiles)
        {
            throw new InvalidOperationException(
                "Backup creation requires overwrite support to be enabled.");
        }

        return new GeneratorFrameworkConfiguration(normalizedOptions);
    }

    private static void ValidateDirectoryPath(
        string value,
        string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "A directory path is required.",
                parameterName);
        }

        try
        {
            _ = Path.GetFullPath(value);
        }
        catch (Exception exception) when (
            exception is ArgumentException or
            NotSupportedException or
            PathTooLongException)
        {
            throw new ArgumentException(
                "The configured directory path is invalid.",
                parameterName,
                exception);
        }
    }
}
