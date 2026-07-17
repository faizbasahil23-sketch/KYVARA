using System.Text.RegularExpressions;
using Kyvara.Builder.Generators.Framework.Abstractions;

namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed partial class TokenReplacer : ITokenReplacer
{
    public string Replace(
        string template,
        IReadOnlyDictionary<string, string> tokens)
    {
        ArgumentNullException.ThrowIfNull(template);
        ArgumentNullException.ThrowIfNull(tokens);

        return PlaceholderPattern().Replace(
            template,
            match =>
            {
                var tokenName =
                    match.Groups["name"].Value;

                return tokens.TryGetValue(
                    tokenName,
                    out var value)
                    ? value ?? string.Empty
                    : match.Value;
            });
    }

    [GeneratedRegex(
        @"\{\{\s*(?<name>[A-Za-z][A-Za-z0-9_.-]*)\s*\}\}",
        RegexOptions.CultureInvariant)]
    private static partial Regex PlaceholderPattern();
}
