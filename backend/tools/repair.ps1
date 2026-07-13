. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA REPAIR"

Write-Log "Repair Started"

function Step($Name)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Cyan
    Write-Host $Name -ForegroundColor Yellow
    Write-Host "===================================================" -ForegroundColor Cyan
}

function RunDotnet($Arguments)
{
    Write-Host ""
    Write-Host "dotnet $Arguments" -ForegroundColor Gray

    Invoke-Expression "dotnet $Arguments"

    if($LASTEXITCODE -ne 0)
    {
        Write-Host ""
        Write-Host "FAILED : dotnet $Arguments" -ForegroundColor Red
        exit 1
    }
}

Step "CLEAN"

RunDotnet "clean `"$Global:SOLUTION`""

Step "NUGET CACHE"

dotnet nuget locals all --clear

Step "RESTORE"

RunDotnet "restore `"$Global:SOLUTION`""

Step "BUILD DEBUG"

RunDotnet "build `"$Global:SOLUTION`" -c Debug --no-restore"

Step "BUILD RELEASE"

RunDotnet "build `"$Global:SOLUTION`" -c Release --no-restore"

Step "EF DATABASE"

try
{
    dotnet ef database update `
    --project "$Global:KYVARA_ROOT\Kyvara.Infrastructure" `
    --startup-project "$Global:KYVARA_ROOT\Kyvara.Api"

    Write-Host ""
    Write-Host "Database OK" -ForegroundColor Green
}
catch
{
    Write-Host ""
    Write-Host "Database Update Failed" -ForegroundColor Yellow
}

Step "PACKAGE"

dotnet list "$Global:SOLUTION" package

Step "OUTDATED"

dotnet list "$Global:SOLUTION" package --outdated

Step "VULNERABLE"

dotnet list "$Global:SOLUTION" package --vulnerable

Write-Host ""
Write-Host "===================================================" -ForegroundColor Green
Write-Host "KYVARA REPAIR FINISHED" -ForegroundColor Green
Write-Host "===================================================" -ForegroundColor Green
Write-Host ""

Write-Log "Repair Finished"
