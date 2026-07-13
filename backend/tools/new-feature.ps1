. "$PSScriptRoot\core\Core.ps1"

param(
    [string]$Name="Feature"
)

Show-Banner "NEW FEATURE"

$Base="$Global:KYVARA_ROOT\Kyvara.Application\$Name"

New-Item "$Base\Commands" -ItemType Directory -Force | Out-Null
New-Item "$Base\Queries" -ItemType Directory -Force | Out-Null
New-Item "$Base\Dtos" -ItemType Directory -Force | Out-Null
New-Item "$Base\Validators" -ItemType Directory -Force | Out-Null

Write-Host ""
Write-Host "Feature Created : $Name" -ForegroundColor Green
