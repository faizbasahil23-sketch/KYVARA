namespace Kyvara.Builder.Generators.Application;

public sealed class DtoGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Dtos;

public sealed class {{Entity}}Dto
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}
""";
    }
}
