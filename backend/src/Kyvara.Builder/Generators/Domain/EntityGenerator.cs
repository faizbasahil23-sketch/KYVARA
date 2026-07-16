namespace Kyvara.Builder.Generators.Domain;

public sealed class EntityGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Entities;

public sealed class {{Entity}}
{
    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private {{Entity}}()
    {
    }

    public {{Entity}}(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }
}
""";
    }
}