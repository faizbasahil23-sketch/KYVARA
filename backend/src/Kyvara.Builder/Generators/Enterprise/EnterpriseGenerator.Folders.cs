namespace Kyvara.Builder.Generators.Enterprise;

public sealed partial class EnterpriseGenerator
{
    private static void CreateFolders(string root,string module)
    {
        Create(root,$"{module}.Domain",EnterpriseFolders.Domain);

        Create(root,$"{module}.Application",EnterpriseFolders.Application);

        Create(root,$"{module}.Infrastructure",EnterpriseFolders.Infrastructure);

        Create(root,$"{module}.Api",EnterpriseFolders.Api);

        Create(root,$"{module}.UnitTests",EnterpriseFolders.Tests);

        Create(root,$"{module}.IntegrationTests",EnterpriseFolders.Tests);

        Create(root,$"{module}.ArchitectureTests",EnterpriseFolders.Tests);
    }

    private static void Create(
        string root,
        string project,
        IEnumerable<string> folders)
    {
        foreach(var folder in folders)
        {
            Directory.CreateDirectory(
                Path.Combine(root,project,folder));
        }
    }
}
