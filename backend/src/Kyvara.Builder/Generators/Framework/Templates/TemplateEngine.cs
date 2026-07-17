using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Templates.Validation;

namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed class TemplateEngine
{
    private readonly ITemplateProvider _templateProvider;
    private readonly ITokenReplacer _tokenReplacer;

    public TemplateEngine(
        ITemplateProvider templateProvider,
        ITokenReplacer tokenReplacer)
    {
        ArgumentNullException.ThrowIfNull(templateProvider);
        ArgumentNullException.ThrowIfNull(tokenReplacer);

        _templateProvider = templateProvider;
        _tokenReplacer = tokenReplacer;
    }

    public async Task<TemplateRenderResult> RenderAsync(
        TemplateRenderRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(
            request.TemplateName);

        cancellationToken.ThrowIfCancellationRequested();

        var template =
            await _templateProvider.GetTemplateAsync(
                request.TemplateName,
                cancellationToken);

        var placeholders =
            TemplatePlaceholderParser.Find(template);

        var unusedTokens =
            TemplateValidator.ValidateTokens(
                template,
                request.Tokens,
                request.RejectUnusedTokens);

        var renderedContent =
            _tokenReplacer.Replace(
                template,
                request.Tokens);

        if (request.RejectUnresolvedPlaceholders)
        {
            TemplateValidator
                .EnsureNoUnresolvedPlaceholders(
                    renderedContent);
        }

        return new TemplateRenderResult(
            request.TemplateName,
            renderedContent,
            placeholders,
            unusedTokens);
    }
}
