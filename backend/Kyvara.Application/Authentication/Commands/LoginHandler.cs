using MediatR;
using Kyvara.Application.Authentication.Dtos;
using Kyvara.Application.Authentication.Interfaces;
using Kyvara.Domain.Users.Repositories;
using Kyvara.Domain.Users.ValueObjects;

namespace Kyvara.Application.Authentication.Commands;

public sealed class LoginHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwt;

    public LoginHandler(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IJwtProvider jwt)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var email = new UserEmail(request.Email);

        var user =
            await _users.GetByEmailAsync(
                email,
                cancellationToken);

        if (user is null)
            throw new Exception("Invalid email or password.");

        if (!_passwordHasher.Verify(
                request.Password,
                user.Password.Value))
            throw new Exception("Invalid email or password.");

        var token =
            _jwt.GenerateToken(
                user.Id.Value,
                user.Email.Value);

        return new LoginResponse(token);
    }
}
