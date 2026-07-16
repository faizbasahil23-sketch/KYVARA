namespace Kyvara.Builder.Generators.Application;

public sealed class QueryGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Queries;

public sealed record Get{{Entity}}ByIdQuery(
    Guid Id
);

public sealed record GetAll{{Entity}}Query();

public sealed record Search{{Entity}}Query(
    string? Keyword
);
""";
    }
}
