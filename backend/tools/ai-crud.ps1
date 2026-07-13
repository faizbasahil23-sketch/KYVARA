. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="Product"
)

Show-Banner "AI CRUD GENERATOR"

$Root="$Global:KYVARA_ROOT\Kyvara.Application\$Name"

New-Item "$Root\Commands\Create" -ItemType Directory -Force | Out-Null
New-Item "$Root\Commands\Update" -ItemType Directory -Force | Out-Null
New-Item "$Root\Commands\Delete" -ItemType Directory -Force | Out-Null
New-Item "$Root\Queries\Get" -ItemType Directory -Force | Out-Null
New-Item "$Root\Queries\List" -ItemType Directory -Force | Out-Null

Write-Host ""
Write-Host "CRUD Structure Created." -ForegroundColor Green
