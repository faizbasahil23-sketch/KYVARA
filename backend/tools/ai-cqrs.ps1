. "$PSScriptRoot\core\Core.ps1"

param([string]$Feature="Sample")

Show-Banner "AI CQRS"

Write-Host "Generating Commands..."
Write-Host "Generating Queries..."
Write-Host "Generating Handlers..."
Write-Host "Generating Validators..."

Start-Sleep 1

Write-Host ""
Write-Host "CQRS Completed." -ForegroundColor Green
