using MediatR;

namespace {{Namespace}}.Commands;

public sealed record Create{{Entity}}Command(
    Guid Id
) : IRequest<Guid>;
