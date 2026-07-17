namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Provides default paths and settings for generated file output.
/// </summary>
public static class DefaultFileWriterConfiguration
{
    public const string DefaultDirectoryName = "Generated";

    /// <summary>
    /// Resolves the default generated output root.
    /// </summary>
    public static string ResolveOutputRoot(string baseDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);

        var fullBaseDirectory = Path.GetFullPath(baseDirectory);
        return Path.Combine(fullBaseDirectory, DefaultDirectoryName);
    }

    /// <summary>
    /// Applies default file writer settings when they were not explicitly configured.
    /// </summary>
    public static void Apply(
        GeneratorFrameworkOptions options,
        string baseDirectory)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);

        if (string.IsNullOrWhiteSpace(options.OutputRoot))
        {
            options.OutputRoot = ResolveOutputRoot(baseDirectory);
        }
        else
        {
            options.OutputRoot = Path.GetFullPath(options.OutputRoot);
        }
    }
}
