using MediatR;

namespace {{Namespace}}.Queries;

public sealed record Get{{Entity}}ByIdQuery(
    Guid Id
) : IRequest<{{Entity}}Dto>;
