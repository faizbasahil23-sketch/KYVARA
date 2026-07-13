. "$PSScriptRoot\core\Core.ps1"

param(
    [string]$Name="Aggregate"
)

Show-Banner "NEW AGGREGATE"

$Folder="$Global:KYVARA_ROOT\Kyvara.Domain\Aggregates"

if(!(Test-Path $Folder))
{
    New-Item $Folder -ItemType Directory | Out-Null
}

@"
namespace Kyvara.Domain.Aggregates;

public sealed class $Name
{

}
"@ | Set-Content "$Folder\$Name.cs"

Write-Host ""
Write-Host "Aggregate Created : $Name" -ForegroundColor Green
