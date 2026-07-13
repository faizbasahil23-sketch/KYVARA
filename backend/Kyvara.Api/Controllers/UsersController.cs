using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Kyvara.Application.Users.Queries.GetCurrentUser;

namespace Kyvara.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IResult> Me(
        CancellationToken cancellationToken)
    {
        var id =
            Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _sender.Send(
                new GetCurrentUserQuery(id),
                cancellationToken);

        return Results.Ok(result);
    }
}
