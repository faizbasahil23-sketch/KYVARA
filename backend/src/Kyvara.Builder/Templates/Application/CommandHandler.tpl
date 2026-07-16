using MediatR;

namespace {{Namespace}}.Handlers;

public sealed class Create{{Entity}}CommandHandler
    : IRequestHandler<Create{{Entity}}Command, Guid>
{
    public async Task<Guid> Handle(
        Create{{Entity}}Command request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return request.Id;
    }
}

public sealed class Update{{Entity}}CommandHandler
    : IRequestHandler<Update{{Entity}}Command>
{
    public async Task Handle(
        Update{{Entity}}Command request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}

public sealed class Delete{{Entity}}CommandHandler
    : IRequestHandler<Delete{{Entity}}Command>
{
    public async Task Handle(
        Delete{{Entity}}Command request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
