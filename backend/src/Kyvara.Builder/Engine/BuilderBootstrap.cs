using Kyvara.Builder.Generators.Domain;

namespace Kyvara.Builder.Engine;

public sealed class BuilderBootstrap
{
    public BuilderEngine Create()
    {
        var registry = new GeneratorRegistry();

        registry.Register(new CompleteDomainGenerator());

        return new BuilderEngine();
    }
}
