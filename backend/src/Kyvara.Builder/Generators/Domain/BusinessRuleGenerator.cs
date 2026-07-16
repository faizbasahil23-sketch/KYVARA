namespace Kyvara.Builder.Generators.Domain;

public sealed class BusinessRuleGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Rules;

public interface IBusinessRule
{
    bool IsBroken();

    string Message { get; }
}
""";
    }
}
