using MediatR;
using Kyvara.Domain.Users.Repositories;

namespace Kyvara.Application.Profile.Queries.GetMyProfile;

public sealed class GetMyProfileHandler
    : IRequestHandler<GetMyProfileQuery, GetMyProfileResponse>
{
    private readonly IUserProfileRepository _profiles;

    public GetMyProfileHandler(
        IUserProfileRepository profiles)
    {
        _profiles = profiles;
    }

    public async Task<GetMyProfileResponse> Handle(
        GetMyProfileQuery request,
        CancellationToken cancellationToken)
    {
        var profile = await _profiles.GetAsync(
            request.UserId,
            cancellationToken);

        if (profile is null)
            throw new Exception("Profile not found.");

        return new GetMyProfileResponse(
            profile.Id,
            profile.FullName,
            profile.Username.Value,
            profile.Bio,
            profile.AvatarUrl,
            profile.TimeZone,
            profile.Language);
    }
}
