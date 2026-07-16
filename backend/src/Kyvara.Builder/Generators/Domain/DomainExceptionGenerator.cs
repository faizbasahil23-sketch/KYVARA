namespace Kyvara.Builder.Generators.Domain;

public sealed class DomainExceptionGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }
}
""";
    }
}
