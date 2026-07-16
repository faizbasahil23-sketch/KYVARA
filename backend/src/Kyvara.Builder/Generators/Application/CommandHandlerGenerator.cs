namespace Kyvara.Builder.Generators.Application;

public sealed class CommandHandlerGenerator
{
    public string Generate()
    {
        return """
using MediatR;

namespace {{Namespace}}.Handlers;

public sealed class Create{{Entity}}CommandHandler
    : IRequestHandler<Create{{Entity}}Command, Guid>
{
    public async Task<Guid> Handle(
        Create{{Entity}}Command request,
        CancellationToken cancellationToken)
    {
        // TODO: Implement create logic
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
        // TODO: Implement update logic
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
        // TODO: Implement delete logic
        await Task.CompletedTask;
    }
}
""";
    }
}
