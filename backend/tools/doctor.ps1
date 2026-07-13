. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA DOCTOR"

Write-Log "Doctor Started"

$Problems=@()

function Check($Name,$Condition)
{
    if($Condition)
    {
        Write-Host "[ OK ] $Name" -ForegroundColor Green
    }
    else
    {
        Write-Host "[FAIL] $Name" -ForegroundColor Red
        $script:Problems+=$Name
    }
}

Write-Host ""
Write-Host "========== PROJECT =========="

Check "Solution" (Test-Path $Global:SOLUTION)
Check "Api" (Test-Path "$Global:KYVARA_ROOT\Kyvara.Api")
Check "Application" (Test-Path "$Global:KYVARA_ROOT\Kyvara.Application")
Check "Domain" (Test-Path "$Global:KYVARA_ROOT\Kyvara.Domain")
Check "Infrastructure" (Test-Path "$Global:KYVARA_ROOT\Kyvara.Infrastructure")

Write-Host ""
Write-Host "========== DATABASE =========="

Check "Database" (Test-Path $Global:DATABASE)

Write-Host ""
Write-Host "========== SDK =========="

dotnet --version

if($LASTEXITCODE -eq 0)
{
    Write-Host "[ OK ] .NET SDK" -ForegroundColor Green
}
else
{
    Write-Host "[FAIL] .NET SDK" -ForegroundColor Red
    $Problems+="Dotnet SDK"
}

Write-Host ""
Write-Host "========== RESTORE =========="

dotnet restore "$Global:SOLUTION"

if($LASTEXITCODE -eq 0)
{
    Write-Host "[ OK ] Restore" -ForegroundColor Green
}
else
{
    Write-Host "[FAIL] Restore" -ForegroundColor Red
    $Problems+="Restore"
}

Write-Host ""
Write-Host "========== BUILD =========="

dotnet build "$Global:SOLUTION" --no-restore

if($LASTEXITCODE -eq 0)
{
    Write-Host "[ OK ] Build" -ForegroundColor Green
}
else
{
    Write-Host "[FAIL] Build" -ForegroundColor Red
    $Problems+="Build"
}

Write-Host ""
Write-Host "========== PACKAGE =========="

dotnet list "$Global:SOLUTION" package --outdated

Write-Host ""
Write-Host "========== SECURITY =========="

dotnet list "$Global:SOLUTION" package --vulnerable

Write-Host ""
Write-Host "========== SUMMARY =========="

if($Problems.Count -eq 0)
{
    Write-Host ""
    Write-Host "PROJECT HEALTHY" -ForegroundColor Green
}
else
{
    Write-Host ""
    Write-Host "Problems :" $Problems.Count -ForegroundColor Yellow
    Write-Host ""

    $Problems | Sort-Object -Unique

    Write-Host ""
    Write-Host "Run repair.ps1" -ForegroundColor Yellow
}

Write-Log "Doctor Finished"

Write-Host ""
