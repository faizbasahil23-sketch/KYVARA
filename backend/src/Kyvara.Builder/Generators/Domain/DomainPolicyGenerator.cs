namespace Kyvara.Builder.Generators.Domain;

public sealed class DomainPolicyGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Policies;

public abstract class DomainPolicy
{
}
""";
    }
}
