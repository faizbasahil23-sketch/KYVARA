using Kyvara.Builder.Services;

namespace Kyvara.Builder.Generators.Enterprise;

public sealed partial class EnterpriseGenerator
{
    private readonly ProjectGenerator _generator = new();

    public async Task GenerateAsync(string root,string module)
    {
        var definition=new EnterpriseModuleDefinition
        {
            Name=module
        };

        foreach(var project in definition.Projects)
        {
            switch(project)
            {
                case "Api":
                    await _generator.CreateWebApiAsync(root,$"{module}.{project}");
                    break;

                case "UnitTests":
                case "IntegrationTests":
                case "ArchitectureTests":
                    await _generator.CreateXunitAsync(root,$"{module}.{project}");
                    break;

                default:
                    await _generator.CreateClassLibraryAsync(root,$"{module}.{project}");
                    break;
            }
        }
    }
}
