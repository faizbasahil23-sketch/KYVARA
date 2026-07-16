namespace Kyvara.Builder.Generators.Application;

public sealed class CommandHandlerGenerator
{
    public string Generate()
    {
        return """
using MediatR;

namespace {{Namespace}}.Commands;

public sealed class Create{{Entity}}CommandHandler
    : IRequestHandler<Create{{Entity}}Command, Guid>
{
    public async Task<Guid> Handle(
        Create{{Entity}}Command request,
        CancellationToken cancellationToken)
    {
        // TODO:
        // Create entity
        // Save using repository
        // Commit UnitOfWork

        return await Task.FromResult(request.Id);
    }
}
""";
    }
}
