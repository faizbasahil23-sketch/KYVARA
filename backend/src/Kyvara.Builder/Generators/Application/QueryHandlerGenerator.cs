namespace Kyvara.Builder.Generators.Application;

public sealed class QueryHandlerGenerator
{
    public string Generate()
    {
        return """
using MediatR;

namespace {{Namespace}}.Queries;

public sealed class Get{{Entity}}ByIdQueryHandler
    : IRequestHandler<Get{{Entity}}ByIdQuery, {{Entity}}Dto>
{
    public async Task<{{Entity}}Dto> Handle(
        Get{{Entity}}ByIdQuery request,
        CancellationToken cancellationToken)
    {
        // TODO:
        // Load entity from repository
        // Map to DTO

        return await Task.FromResult(new {{Entity}}Dto());
    }
}
""";
    }
}
