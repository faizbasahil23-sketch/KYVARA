namespace Kyvara.Domain.Users.ValueObjects;

/// <summary>
/// Strongly typed User identifier.
/// </summary>
public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
