using MediatR;

namespace Kyvara.Application.Profile.Queries.GetMyProfile;

public sealed record GetMyProfileQuery(Guid UserId)
    : IRequest<GetMyProfileResponse>;

public sealed record GetMyProfileResponse(
    Guid Id,
    string? FullName,
    string? UserName,
    string? Bio,
    string? AvatarUrl,
    string? TimeZone,
    string? Language);
