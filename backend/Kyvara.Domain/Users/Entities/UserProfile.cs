using Kyvara.Domain.Users.ValueObjects;

namespace Kyvara.Domain.Users.Entities;

public sealed class UserProfile
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Username Username { get; private set; } = null!;

    public string FullName { get; private set; } = "";

    public string Bio { get; private set; } = "";

    public string AvatarUrl { get; private set; } = "";

    public string TimeZone { get; private set; } = "Asia/Jakarta";

    public string Language { get; private set; } = "id";

    private UserProfile()
    {
    }

    public static UserProfile Create(
        Guid userId,
        Username username,
        string fullName)
    {
        return new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Username = username,
            FullName = fullName
        };
    }

    public void Update(
        string fullName,
        string bio,
        string avatarUrl,
        string timezone,
        string language)
    {
        FullName = fullName;
        Bio = bio;
        AvatarUrl = avatarUrl;
        TimeZone = timezone;
        Language = language;
    }
}
