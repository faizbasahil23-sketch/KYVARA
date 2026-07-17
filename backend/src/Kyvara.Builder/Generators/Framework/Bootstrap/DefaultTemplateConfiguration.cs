namespace Kyvara.Builder.Generators.Framework.Bootstrap;

/// <summary>
/// Provides default paths and settings for the template subsystem.
/// </summary>
public static class DefaultTemplateConfiguration
{
    public const string DefaultDirectoryName = "Templates";

    /// <summary>
    /// Resolves the default template root beneath the supplied base directory.
    /// </summary>
    public static string ResolveTemplateRoot(string baseDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);

        var fullBaseDirectory = Path.GetFullPath(baseDirectory);
        return Path.Combine(fullBaseDirectory, DefaultDirectoryName);
    }

    /// <summary>
    /// Applies default template settings when they were not explicitly configured.
    /// </summary>
    public static void Apply(
        GeneratorFrameworkOptions options,
        string baseDirectory)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);

        if (string.IsNullOrWhiteSpace(options.TemplateRoot))
        {
            options.TemplateRoot = ResolveTemplateRoot(baseDirectory);
        }
        else
        {
            options.TemplateRoot = Path.GetFullPath(options.TemplateRoot);
        }
    }
}
