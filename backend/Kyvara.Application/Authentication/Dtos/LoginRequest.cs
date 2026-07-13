namespace Kyvara.Application.Authentication.Dtos;

/// <summary>
/// Login request.
/// </summary>
public sealed record LoginRequest(
    string Email,
    string Password);
