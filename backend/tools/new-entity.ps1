. "$PSScriptRoot\core\Core.ps1"

param(
    [string]$Name="Entity"
)

Show-Banner "NEW ENTITY"

$Folder="$Global:KYVARA_ROOT\Kyvara.Domain\Entities"

if(!(Test-Path $Folder))
{
    New-Item $Folder -ItemType Directory | Out-Null
}

@"
namespace Kyvara.Domain.Entities;

public sealed class $Name
{

}
"@ | Set-Content "$Folder\$Name.cs"

Write-Host ""
Write-Host "Entity Created : $Name" -ForegroundColor Green
