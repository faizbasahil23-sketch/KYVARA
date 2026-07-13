namespace Kyvara.Domain.Users.ValueObjects;

public sealed record Username
{
    public string Value { get; }

    public Username(string value)
    {
        value = value.Trim();

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Username is required.");

        if (value.Length < 3)
            throw new ArgumentException("Username minimum 3 characters.");

        Value = value;
    }

    public override string ToString()
        => Value;

    public static implicit operator string(Username username)
        => username.Value;
}
