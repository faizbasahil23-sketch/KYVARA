using System.Security.Claims;
using Kyvara.Application.Profile.Commands.UpdateProfile;
using Kyvara.Application.Profile.Queries.GetMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kyvara.Api.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public sealed class ProfileController : ControllerBase
{
    private readonly ISender _sender;

    public ProfileController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IResult> Get(
        CancellationToken cancellationToken)
    {
        var id = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _sender.Send(
            new GetMyProfileQuery(id),
            cancellationToken);

        return Results.Ok(result);
    }

    [HttpPut]
    public async Task<IResult> Update(
        UpdateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var id = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Console.WriteLine("========== UPDATE ==========");
        Console.WriteLine(id);

        await _sender.Send(
            new UpdateProfileCommand(
                id,
                request.FullName,
                request.Bio,
                request.AvatarUrl,
                request.TimeZone,
                request.Language),
            cancellationToken);

        return Results.NoContent();
    }
}

public sealed record UpdateProfileRequest(
    string FullName,
    string Bio,
    string AvatarUrl,
    string TimeZone,
    string Language);