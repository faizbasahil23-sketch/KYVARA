using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Tests.GeneratorFramework;

public static class TemplateEngineTest
{
    public static async Task<bool> RunAsync()
    {
        var templates =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["Entity.tpl"] =
                    """
                    namespace {{Namespace}}.Entities;

                    public sealed class {{EntityName}}
                    {
                    }
                    """
            };

        var provider =
            new EmbeddedTemplateProvider(templates);

        var replacer = new TokenReplacer();

        var engine = new TemplateEngine(
            provider,
            replacer);

        var request = new TemplateRenderRequest
        {
            TemplateName = "Entity.tpl",
            Tokens = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["Namespace"] = "Sample.Domain",
                ["EntityName"] = "Customer"
            }
        };

        var result = await engine.RenderAsync(request);

        return
            result.Content.Contains(
                "namespace Sample.Domain.Entities;",
                StringComparison.Ordinal) &&
            result.Content.Contains(
                "class Customer",
                StringComparison.Ordinal) &&
            result.UsedTokens.Count == 2 &&
            result.UnusedTokens.Count == 0;
    }

    public static async Task<bool> RejectsMissingTokensAsync()
    {
        var provider =
            new EmbeddedTemplateProvider(
                new Dictionary<string, string>
                {
                    ["Entity.tpl"] =
                        "public sealed class {{EntityName}} { }"
                });

        var engine = new TemplateEngine(
            provider,
            new TokenReplacer());

        try
        {
            await engine.RenderAsync(
                new TemplateRenderRequest
                {
                    TemplateName = "Entity.tpl",
                    Tokens =
                        new Dictionary<string, string>()
                });

            return false;
        }
        catch (TemplateException)
        {
            return true;
        }
    }
}
