namespace Kyvara.Api.Requests;

public sealed record LoginRequest(
    string Email,
    string Password);
