using Kyvara.Builder.Services;

namespace Kyvara.Builder.Generators;

public sealed class ModuleGenerator
{
    private readonly ProjectGenerator _projects=new();

    public async Task GenerateAsync(
        string root,
        string module)
    {
        await _projects.CreateClassLibraryAsync(root,$"{module}.Domain");

        await _projects.CreateClassLibraryAsync(root,$"{module}.Application");

        await _projects.CreateClassLibraryAsync(root,$"{module}.Infrastructure");

        await _projects.CreateWebApiAsync(root,$"{module}.Api");

        await _projects.CreateClassLibraryAsync(root,$"{module}.Contracts");

        await _projects.CreateXunitAsync(root,$"{module}.UnitTests");

        await _projects.CreateXunitAsync(root,$"{module}.IntegrationTests");
    }
}
