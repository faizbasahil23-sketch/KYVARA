using Microsoft.AspNetCore.Mvc;

namespace Kyvara.Api.Controllers;

[ApiController]
[Route("api/system")]
public sealed class SystemController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "Healthy",
            utc = DateTime.UtcNow
        });
    }

    [HttpGet("version")]
    public IActionResult Version()
    {
        return Ok(new
        {
            application = "KYVARA",
            version = "0.1.0",
            framework = ".NET 10"
        });
    }
}
