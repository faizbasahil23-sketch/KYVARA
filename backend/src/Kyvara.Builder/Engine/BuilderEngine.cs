namespace Kyvara.Builder.Engine;

public sealed class BuilderEngine
{
    private readonly ModuleWriter _moduleWriter = new();

    public void BuildModule(
        string outputPath,
        string moduleName,
        string @namespace)
    {
        _moduleWriter.WriteModule(
            outputPath,
            moduleName,
            @namespace);
    }
}
