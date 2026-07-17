namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface IGeneratorRegistry
{
    void Register(IGenerator generator);

    bool Contains(string generatorName);

    IGenerator Resolve(string generatorName);

    IReadOnlyCollection<string> GetRegisteredNames();
}
