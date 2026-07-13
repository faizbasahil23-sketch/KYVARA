. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="Sample"
)

Show-Banner "API CONTROLLER"

$Folder="$Global:KYVARA_ROOT\Kyvara.Api\Controllers"

@"
using Microsoft.AspNetCore.Mvc;

namespace Kyvara.Api.Controllers;

[ApiController]
[Route("api/$($Name.ToLower())")]
public sealed class ${Name}Controller : ControllerBase
{

}
"@ | Set-Content "$Folder\$Name`Controller.cs"

Write-Host "Controller Created"

