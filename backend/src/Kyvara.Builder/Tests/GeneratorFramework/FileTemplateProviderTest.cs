using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Tests.GeneratorFramework;

public static class FileTemplateProviderTest
{
    public static async Task<bool> RunAsync()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            "kyvara-template-provider-test",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);

        try
        {
            var filePath = Path.Combine(
                root,
                "Sample.tpl");

            await File.WriteAllTextAsync(
                filePath,
                "Hello {{Name}}");

            var provider =
                new FileTemplateProvider(
                    root,
                    enableCache: true);

            var first =
                await provider.GetTemplateAsync(
                    "Sample.tpl");

            var second =
                await provider.GetTemplateAsync(
                    "Sample.tpl");

            return
                first == "Hello {{Name}}" &&
                second == first &&
                provider.CachedTemplateCount == 1 &&
                provider.Exists("Sample.tpl");
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(
                    root,
                    recursive: true);
            }
        }
    }

    public static bool RejectsPathTraversal()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            "kyvara-template-path-test");

        var provider =
            new FileTemplateProvider(root);

        try
        {
            provider.Exists(
                ".." +
                Path.DirectorySeparatorChar +
                "Secret.tpl");

            return false;
        }
        catch (TemplateException)
        {
            return true;
        }
    }
}
