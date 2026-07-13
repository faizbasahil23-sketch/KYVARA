using MediatR;
using Kyvara.Application.Authentication.Dtos;

namespace Kyvara.Application.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken)
    : IRequest<LoginResponse>;
