using MediatR;

namespace Kyvara.Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string FullName,
    string Email,
    string Password)
    : IRequest<Guid>;
