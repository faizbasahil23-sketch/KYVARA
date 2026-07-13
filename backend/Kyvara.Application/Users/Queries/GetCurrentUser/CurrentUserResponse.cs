namespace Kyvara.Application.Users.Queries.GetCurrentUser;

public sealed record CurrentUserResponse(
    Guid Id,
    string Email);
