namespace Kyvara.Application.Authentication.Interfaces;

/// <summary>
/// Password hashing abstraction.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(
        string password,
        string hash);
}
