namespace Kyvara.Application.Authentication.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(
        Guid userId,
        string email);
}
