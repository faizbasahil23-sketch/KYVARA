namespace Kyvara.Builder.Generators.Application;

public sealed class QueryGenerator
{
    public string Generate()
    {
        return """
using MediatR;

namespace {{Namespace}}.Queries;

public sealed record Get{{Entity}}ByIdQuery(
    Guid Id
) : IRequest<{{Entity}}Dto>;
""";
    }
}
