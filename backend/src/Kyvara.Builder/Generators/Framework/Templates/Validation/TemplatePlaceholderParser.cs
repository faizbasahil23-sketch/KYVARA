using System.Text.RegularExpressions;

namespace Kyvara.Builder.Generators.Framework.Templates.Validation;

public static partial class TemplatePlaceholderParser
{
    public static IReadOnlyCollection<string> Find(
        string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        return PlaceholderPattern()
            .Matches(content)
            .Select(match => match.Groups["name"].Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(
                name => name,
                StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    [GeneratedRegex(
        @"\{\{\s*(?<name>[A-Za-z][A-Za-z0-9_.-]*)\s*\}\}",
        RegexOptions.CultureInvariant)]
    private static partial Regex PlaceholderPattern();
}
