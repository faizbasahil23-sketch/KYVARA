namespace Kyvara.Builder.Generators.Domain;

public sealed class DomainServiceGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Services;

public abstract class DomainService
{
}
""";
    }
}
