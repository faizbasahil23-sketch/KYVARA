. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="DomainEvent"
)

Show-Banner "DOMAIN EVENT"

$Folder="$Global:KYVARA_ROOT\Kyvara.Domain\DomainEvents"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
namespace Kyvara.Domain.DomainEvents;

public sealed record $Name;
"@ | Set-Content "$Folder\$Name.cs"

Write-Host "Domain Event Created"

