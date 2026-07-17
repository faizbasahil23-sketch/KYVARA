namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface ITemplateProvider
{
    bool Exists(string templateName);

    Task<string> GetTemplateAsync(
        string templateName,
        CancellationToken cancellationToken = default);
}
