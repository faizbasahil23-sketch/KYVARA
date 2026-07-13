using MediatR;
using Kyvara.Application.Authentication.Dtos;

namespace Kyvara.Application.Authentication.Commands;

public sealed record LoginCommand(
    string Email,
    string Password)
    : IRequest<LoginResponse>;
