. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="ValueObject"
)

Show-Banner "VALUE OBJECT"

$Folder="$Global:KYVARA_ROOT\Kyvara.Domain\ValueObjects"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
namespace Kyvara.Domain.ValueObjects;

public sealed record $Name;
"@ | Set-Content "$Folder\$Name.cs"

Write-Host "Value Object Created"

