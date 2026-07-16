using MediatR;

namespace {{Namespace}}.Handlers;

public sealed class Get{{Entity}}ByIdQueryHandler
    : IRequestHandler<Get{{Entity}}ByIdQuery, {{Entity}}Dto?>
{
    public async Task<{{Entity}}Dto?> Handle(
        Get{{Entity}}ByIdQuery request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return default;
    }
}

public sealed class GetAll{{Entity}}QueryHandler
    : IRequestHandler<GetAll{{Entity}}Query,IReadOnlyList<{{Entity}}Dto>>
{
    public async Task<IReadOnlyList<{{Entity}}Dto>> Handle(
        GetAll{{Entity}}Query request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return [];
    }
}

public sealed class Search{{Entity}}QueryHandler
    : IRequestHandler<Search{{Entity}}Query,IReadOnlyList<{{Entity}}Dto>>
{
    public async Task<IReadOnlyList<{{Entity}}Dto>> Handle(
        Search{{Entity}}Query request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return [];
    }
}
