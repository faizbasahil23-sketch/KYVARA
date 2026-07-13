using Kyvara.Application.Authentication.Commands;
using Kyvara.Application.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kyvara.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IResult> Register(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var id =
            await _sender.Send(
                command,
                cancellationToken);

        return Results.Ok(new
        {
            UserId = id
        });
    }

    [HttpPost("login")]
    public async Task<IResult> Login(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _sender.Send(
                command,
                cancellationToken);

        return Results.Ok(result);
    }
}
