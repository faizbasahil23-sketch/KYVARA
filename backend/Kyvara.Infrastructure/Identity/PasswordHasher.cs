using BCrypt.Net;
using Kyvara.Application.Authentication.Interfaces;

namespace Kyvara.Infrastructure.Identity;

public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(
        string password,
        string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
