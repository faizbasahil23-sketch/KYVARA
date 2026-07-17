namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed record TemplateRenderResult(
    string TemplateName,
    string Content,
    IReadOnlyCollection<string> UsedTokens,
    IReadOnlyCollection<string> UnusedTokens);
