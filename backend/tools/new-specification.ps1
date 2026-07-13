. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="Specification"
)

Show-Banner "SPECIFICATION"

$Folder="$Global:KYVARA_ROOT\Kyvara.Domain\Specifications"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
namespace Kyvara.Domain.Specifications;

public sealed class $Name
{

}
"@ | Set-Content "$Folder\$Name.cs"

Write-Host "Specification Created"

