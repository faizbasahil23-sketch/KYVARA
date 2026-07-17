namespace Kyvara.Builder.Generators.Framework.FileSystem.Validation;

public static class GeneratedContentNormalizer
{
    public static string Normalize(
        string content,
        bool ensureTrailingNewLine)
    {
        ArgumentNullException.ThrowIfNull(content);

        var normalized = content
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace(
                "\n",
                Environment.NewLine);

        if (!ensureTrailingNewLine)
        {
            return normalized;
        }

        return normalized.EndsWith(
            Environment.NewLine,
            StringComparison.Ordinal)
            ? normalized
            : normalized + Environment.NewLine;
    }
}
