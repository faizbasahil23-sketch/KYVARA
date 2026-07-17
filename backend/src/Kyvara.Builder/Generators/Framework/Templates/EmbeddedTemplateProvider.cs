using Kyvara.Builder.Generators.Framework.Abstractions;

namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed class EmbeddedTemplateProvider : ITemplateProvider
{
    private readonly IReadOnlyDictionary<string, string> _templates;

    public EmbeddedTemplateProvider(
        IReadOnlyDictionary<string, string> templates)
    {
        ArgumentNullException.ThrowIfNull(templates);

        _templates = new Dictionary<string, string>(
            templates,
            StringComparer.OrdinalIgnoreCase);
    }

    public bool Exists(string templateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);

        return _templates.ContainsKey(templateName);
    }

    public Task<string> GetTemplateAsync(
        string templateName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);

        cancellationToken.ThrowIfCancellationRequested();

        if (!_templates.TryGetValue(
                templateName,
                out var template))
        {
            throw new TemplateException(
                $"Embedded template '{templateName}' was not found.");
        }

        return Task.FromResult(template);
    }
}
