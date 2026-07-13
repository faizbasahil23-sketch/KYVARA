using MediatR;
using Kyvara.Domain.Users.Repositories;
using Kyvara.Domain.Users.ValueObjects;

namespace Kyvara.Application.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserHandler
    : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    private readonly IUserRepository _repository;

    public GetCurrentUserHandler(
        IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<CurrentUserResponse> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user =
            await _repository.GetByIdAsync(
                new UserId(request.UserId),
                cancellationToken);

        if (user is null)
            throw new Exception("User not found.");

        return new CurrentUserResponse(
            user.Id.Value,
            user.Email.Value);
    }
}
