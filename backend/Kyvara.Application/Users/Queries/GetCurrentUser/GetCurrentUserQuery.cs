using MediatR;

namespace Kyvara.Application.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(
    Guid UserId)
    : IRequest<CurrentUserResponse>;
