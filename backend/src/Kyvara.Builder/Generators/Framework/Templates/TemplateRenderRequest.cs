namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed class TemplateRenderRequest
{
    public required string TemplateName { get; init; }

    public IReadOnlyDictionary<string, string> Tokens { get; init; } =
        new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

    public bool RejectUnusedTokens { get; init; }

    public bool RejectUnresolvedPlaceholders { get; init; } = true;
}
