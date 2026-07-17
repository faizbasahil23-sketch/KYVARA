namespace Kyvara.Builder.Generators.Framework.FileSystem.Validation;

public static class GeneratedFilePathGuard
{
    public static string ResolveSafePath(
        string outputDirectory,
        string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        if (Path.IsPathRooted(relativePath))
        {
            throw new GeneratedFileWriterException(
                "Generated file path must be relative.");
        }

        var normalizedRelativePath = relativePath
            .Replace(
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);

        var segments = normalizedRelativePath.Split(
            Path.DirectorySeparatorChar,
            StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0)
        {
            throw new GeneratedFileWriterException(
                "Generated file path cannot be empty.");
        }

        if (segments.Any(segment =>
                segment is "." or ".."))
        {
            throw new GeneratedFileWriterException(
                "Generated file path cannot contain directory traversal.");
        }

        if (segments.Any(segment =>
                segment.IndexOfAny(
                    Path.GetInvalidFileNameChars()) >= 0))
        {
            throw new GeneratedFileWriterException(
                $"Generated file path '{relativePath}' contains invalid characters.");
        }

        var rootPath = Path.GetFullPath(outputDirectory);

        var candidatePath = Path.GetFullPath(
            Path.Combine(rootPath, normalizedRelativePath));

        var rootPrefix = rootPath.EndsWith(
            Path.DirectorySeparatorChar)
            ? rootPath
            : rootPath + Path.DirectorySeparatorChar;

        if (!candidatePath.StartsWith(
                rootPrefix,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new GeneratedFileWriterException(
                $"Generated file path '{relativePath}' is outside the output directory.");
        }

        return candidatePath;
    }

    public static string NormalizeRelativePath(
        string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        return relativePath
            .Replace('\\', '/')
            .TrimStart('/');
    }
}
