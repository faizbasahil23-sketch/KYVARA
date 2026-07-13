. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="Repository"
)

Show-Banner "REPOSITORY"

$Folder="$Global:KYVARA_ROOT\Kyvara.Domain\Repositories"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
namespace Kyvara.Domain.Repositories;

public interface I$Name
{

}
"@ | Set-Content "$Folder\I$Name.cs"

Write-Host ""
Write-Host "Repository Created"
