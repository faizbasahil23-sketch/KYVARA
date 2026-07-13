. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="IntegrationEvent"
)

Show-Banner "INTEGRATION EVENT"

$Folder="$Global:KYVARA_ROOT\Kyvara.Application\IntegrationEvents"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
namespace Kyvara.Application.IntegrationEvents;

public sealed record $Name;
"@ | Set-Content "$Folder\$Name.cs"

Write-Host "Integration Event Created"

