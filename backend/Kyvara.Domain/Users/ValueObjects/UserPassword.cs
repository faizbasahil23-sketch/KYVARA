namespace Kyvara.Domain.Users.ValueObjects;

/// <summary>
/// Password hash.
/// </summary>
public sealed class UserPassword
{
    public string Value { get; }

    public UserPassword(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash is required.");

        Value = hash;
    }

    public override string ToString() => "********";
}
