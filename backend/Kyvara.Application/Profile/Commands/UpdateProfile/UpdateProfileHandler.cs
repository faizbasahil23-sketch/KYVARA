using MediatR;
using Kyvara.Domain.Users.Repositories;

namespace Kyvara.Application.Profile.Commands.UpdateProfile;

public sealed class UpdateProfileHandler
    : IRequestHandler<UpdateProfileCommand>
{
    private readonly IUserProfileRepository _profiles;

    public UpdateProfileHandler(
        IUserProfileRepository profiles)
    {
        _profiles = profiles;
    }

    public async Task<Unit> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profile = await _profiles.GetAsync(
            request.UserId,
            cancellationToken);

        Console.WriteLine("Handler UserId:");
        Console.WriteLine(request.UserId);

        Console.WriteLine(profile == null
            ? "PROFILE NULL"
            : "PROFILE FOUND");

        if (profile is null)
            throw new Exception("Profile not found.");

        profile.Update(
            request.FullName,
            request.Bio,
            request.AvatarUrl,
            request.TimeZone,
            request.Language);

        await _profiles.UpdateAsync(
            profile,
            cancellationToken);

        return Unit.Value;
    }
}
