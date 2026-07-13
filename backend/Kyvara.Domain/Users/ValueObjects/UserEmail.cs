using System.Text.RegularExpressions;

namespace Kyvara.Domain.Users.ValueObjects;

/// <summary>
/// User email.
/// </summary>
public sealed class UserEmail
{
    public string Value { get; }

    public UserEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.");

        value = value.Trim().ToLowerInvariant();

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.");

        Value = value;
    }

    public override string ToString() => Value;
}
