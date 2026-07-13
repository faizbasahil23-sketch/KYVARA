using MediatR;

namespace Kyvara.Application.Profile.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
    Guid UserId,
    string FullName,
    string Bio,
    string AvatarUrl,
    string TimeZone,
    string Language)
    : IRequest;