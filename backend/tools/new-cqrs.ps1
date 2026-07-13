. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Feature="Sample"
)

Show-Banner "CQRS GENERATOR"

$Root="$Global:KYVARA_ROOT\Kyvara.Application\$Feature"

New-Item "$Root\Commands" -ItemType Directory -Force | Out-Null
New-Item "$Root\Queries" -ItemType Directory -Force | Out-Null
New-Item "$Root\Validators" -ItemType Directory -Force | Out-Null

Write-Host ""
Write-Host "CQRS Scaffold Created"
