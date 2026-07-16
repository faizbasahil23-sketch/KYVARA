namespace Kyvara.Builder.Services;

public sealed class AuthenticationGenerator
{
    public string GenerateJwtOptions()
    {
        return
"""
namespace Generated.Authentication;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "";

    public string Audience { get; set; } = "";

    public string SecretKey { get; set; } = "";

    public int ExpirationMinutes { get; set; } = 60;
}
""";
    }

    public string GenerateJwtService()
    {
        return
"""
namespace Generated.Authentication;

public sealed class JwtService
{
    public string GenerateToken()
    {
        return "JWT_TOKEN";
    }
}
""";
    }

    public string GenerateTokenProvider()
    {
        return
"""
namespace Generated.Authentication;

public interface ITokenProvider
{
    string GenerateToken();
}
""";
    }
}
