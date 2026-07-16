namespace Kyvara.Builder.Generators.Application;

public sealed class CommandGenerator
{
    public string Generate()
    {
        return """
using MediatR;

namespace {{Namespace}}.Commands;

public sealed record Create{{Entity}}Command(
    Guid Id
) : IRequest<Guid>;
""";
    }
}
