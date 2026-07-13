function Show-Banner($Title)
{
Clear-Host

Write-Host ""
Write-Host "===================================================" -ForegroundColor Cyan
Write-Host $Title -ForegroundColor Yellow
Write-Host "===================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Root      :" $Global:KYVARA_ROOT
Write-Host "Solution  :" $Global:SOLUTION
Write-Host ""
}
