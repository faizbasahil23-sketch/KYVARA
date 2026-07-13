$ErrorActionPreference="SilentlyContinue"

$Root=Split-Path $PSScriptRoot -Parent

Set-Location $Root

$LogFolder="$Root\logs"

if(!(Test-Path $LogFolder))
{
    New-Item $LogFolder -ItemType Directory | Out-Null
}

$Log="$LogFolder\monitor.log"

$HealthUrl="http://localhost:5000/health"

function Log($text)
{
    "$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')  $text" |
    Out-File $Log -Append
}

while($true)
{

Clear-Host

Write-Host ""
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "            KYVARA LIVE MONITOR"
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host ""

$dotnet=Get-Process dotnet -ErrorAction SilentlyContinue

if($dotnet)
{
    $cpu=[math]::Round(($dotnet | Measure-Object CPU -Sum).Sum,2)

    $ram=[math]::Round(($dotnet | Measure-Object WorkingSet64 -Sum).Sum/1MB,2)

    Write-Host "STATUS      : RUNNING" -ForegroundColor Green
    Write-Host "PROCESS     : $($dotnet.Count)"
    Write-Host "CPU TIME    : $cpu sec"
    Write-Host "RAM         : $ram MB"
}
else
{
    Write-Host "STATUS      : STOPPED" -ForegroundColor Red
    Write-Host ""
    Write-Host "Restarting API..."

    Start-Process powershell `
        "-NoExit -Command cd '$Root'; dotnet run --project .\Kyvara.Api"

    Log "API restarted"
}

$db="$Root\Kyvara.Api\kyvara.db"

if(Test-Path $db)
{
    $size=[math]::Round((Get-Item $db).Length/1KB,2)

    Write-Host ""
    Write-Host "DATABASE    : OK"
    Write-Host "SIZE        : $size KB"
}
else
{
    Write-Host ""
    Write-Host "DATABASE    : NOT FOUND" -ForegroundColor Yellow
    Log "Database missing"
}

try
{
    $r=Invoke-WebRequest $HealthUrl -TimeoutSec 2

    Write-Host ""
    Write-Host "API HEALTH  : ONLINE" -ForegroundColor Green
}
catch
{
    Write-Host ""
    Write-Host "API HEALTH  : OFFLINE" -ForegroundColor Red
    Log "Health endpoint offline"
}

$build=(Get-ChildItem -Recurse *.dll | Measure-Object).Count

Write-Host ""
Write-Host "DLL FILES   : $build"

$cs=(Get-ChildItem -Recurse *.cs | Measure-Object).Count

Write-Host "CS FILES    : $cs"

$projects=(Get-ChildItem -Recurse *.csproj | Measure-Object).Count

Write-Host "PROJECTS    : $projects"

$errors=Get-ChildItem "$LogFolder" *.log -ErrorAction SilentlyContinue

Write-Host "LOG FILES   : $($errors.Count)"

Write-Host ""
Write-Host "Updated : $(Get-Date)"

Start-Sleep 5

}
