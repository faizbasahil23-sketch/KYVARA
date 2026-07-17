using Kyvara.Builder.Generators.Domain;

namespace Kyvara.Builder.Engine;

public sealed class ModuleWriter
{
    private readonly ProjectWriter _projectWriter = new();
    private readonly CompleteDomainGenerator _domainGenerator = new();

    public void WriteModule(
        string rootPath,
        string moduleName,
        string @namespace)
    {
        var files = _domainGenerator.Generate(
            @namespace,
            moduleName);

        _projectWriter.WriteProject(rootPath, files);
    }
}
