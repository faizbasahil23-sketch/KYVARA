namespace Kyvara.Builder.Generators.Framework.Templates.Validation;

public static class TemplatePathGuard
{
    public static string ResolveSafePath(
        string rootDirectory,
        string templateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);

        if (Path.IsPathRooted(templateName))
        {
            throw new TemplateException(
                "Template name must be a relative path.");
        }

        var normalizedName = templateName
            .Replace(
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);

        var rootPath = Path.GetFullPath(rootDirectory);

        var candidatePath = Path.GetFullPath(
            Path.Combine(rootPath, normalizedName));

        var rootPrefix = rootPath.EndsWith(
            Path.DirectorySeparatorChar)
            ? rootPath
            : rootPath + Path.DirectorySeparatorChar;

        if (!candidatePath.StartsWith(
                rootPrefix,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new TemplateException(
                $"Template path '{templateName}' is outside the template root.");
        }

        return candidatePath;
    }
}
