. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA WATCH ENGINE"

Write-Log "Watch Started"

$Api="$Global:KYVARA_ROOT\Kyvara.Api"

if(!(Test-Path $Api))
{
    Write-Host "Kyvara.Api not found." -ForegroundColor Red
    exit
}

Push-Location $Api

Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " HOT RELOAD ENABLED" -ForegroundColor Yellow
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Monitoring changes..."

dotnet watch run

Pop-Location

Write-Log "Watch Finished"
