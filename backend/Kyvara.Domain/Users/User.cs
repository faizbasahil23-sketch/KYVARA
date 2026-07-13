using Kyvara.SharedKernel.Common;
using Kyvara.Domain.Users.Events;
using Kyvara.Domain.Users.ValueObjects;

namespace Kyvara.Domain.Users;

/// <summary>
/// User aggregate root.
/// </summary>
public sealed class User : AggregateRoot<UserId>
{
    private User(
        UserId id,
        UserEmail email,
        UserPassword password,
        DateTime registeredAt)
        : base(id)
    {
        Email = email;
        Password = password;
        RegisteredAt = registeredAt;
    }

    public UserEmail Email { get; private set; }

    public UserPassword Password { get; private set; }

    public DateTime RegisteredAt { get; }

    public static User Register(
        UserEmail email,
        UserPassword password,
        DateTime nowUtc)
    {
        var user = new User(
            UserId.New(),
            email,
            password,
            nowUtc);

        user.Raise(
            new UserRegisteredDomainEvent(
                user.Id,
                user.Email));

        return user;
    }

    public void ChangePassword(UserPassword password)
    {
        Password = password;
    }

    public void ChangeEmail(UserEmail email)
    {
        Email = email;
    }
}
