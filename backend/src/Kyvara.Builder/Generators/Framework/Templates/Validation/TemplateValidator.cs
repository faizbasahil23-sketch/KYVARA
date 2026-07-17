namespace Kyvara.Builder.Generators.Framework.Templates.Validation;

public static class TemplateValidator
{
    public static IReadOnlyCollection<string> ValidateTokens(
        string template,
        IReadOnlyDictionary<string, string> tokens,
        bool rejectUnusedTokens)
    {
        ArgumentNullException.ThrowIfNull(template);
        ArgumentNullException.ThrowIfNull(tokens);

        var placeholders =
            TemplatePlaceholderParser.Find(template);

        var missingTokens = placeholders
            .Where(placeholder =>
                !tokens.TryGetValue(placeholder, out var value) ||
                value is null)
            .ToArray();

        if (missingTokens.Length > 0)
        {
            throw new TemplateException(
                "Missing template token(s): " +
                string.Join(", ", missingTokens));
        }

        var unusedTokens = tokens.Keys
            .Where(token =>
                !placeholders.Contains(
                    token,
                    StringComparer.OrdinalIgnoreCase))
            .OrderBy(
                token => token,
                StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (rejectUnusedTokens &&
            unusedTokens.Length > 0)
        {
            throw new TemplateException(
                "Unused template token(s): " +
                string.Join(", ", unusedTokens));
        }

        return unusedTokens;
    }

    public static void EnsureNoUnresolvedPlaceholders(
        string renderedContent)
    {
        ArgumentNullException.ThrowIfNull(renderedContent);

        var unresolved =
            TemplatePlaceholderParser.Find(renderedContent);

        if (unresolved.Count > 0)
        {
            throw new TemplateException(
                "Unresolved template placeholder(s): " +
                string.Join(", ", unresolved));
        }
    }
}
