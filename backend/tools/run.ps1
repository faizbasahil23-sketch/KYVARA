Clear-Host

$ErrorActionPreference="Continue"

function Section($title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Cyan
    Write-Host "===================================================" -ForegroundColor Yellow
}

Section "KYVARA RUN v1.0"

$ApiProject = ".\Kyvara.Api"

#---------------------------------------------------------

Section "CHECK PROJECT"

if(!(Test-Path $ApiProject))
{
    Write-Host "Kyvara.Api tidak ditemukan." -ForegroundColor Red
    exit
}

#---------------------------------------------------------

Section "RESTORE"

dotnet restore

if($LASTEXITCODE -ne 0)
{
    Write-Host "Restore gagal." -ForegroundColor Red
    exit
}

#---------------------------------------------------------

Section "BUILD"

dotnet build

if($LASTEXITCODE -ne 0)
{
    Write-Host "Build gagal." -ForegroundColor Red
    exit
}

#---------------------------------------------------------

Section "DATABASE UPDATE"

dotnet ef database update `
--project .\Kyvara.Infrastructure `
--startup-project .\Kyvara.Api

#---------------------------------------------------------

Section "CHECK DATABASE"

Get-ChildItem -Recurse *.db

#---------------------------------------------------------

Section "CHECK PORT"

$ports = @(5000,5001,7000,7001,8080)

foreach($p in $ports)
{
    $busy = Get-NetTCPConnection -LocalPort $p -ErrorAction SilentlyContinue

    if($busy)
    {
        Write-Host "Port $p sedang digunakan." -ForegroundColor Yellow
    }
    else
    {
        Write-Host "Port $p tersedia." -ForegroundColor Green
    }
}

#---------------------------------------------------------

Section "START API"

Start-Process powershell `
-ArgumentList "-NoExit","-Command","cd '$PWD'; dotnet run --project .\Kyvara.Api"

#---------------------------------------------------------

Section "WAIT"

Start-Sleep -Seconds 8

#---------------------------------------------------------

Section "OPEN SWAGGER"

$urls=@(
"http://localhost:5000/swagger",
"https://localhost:5001/swagger",
"http://localhost:7000/swagger",
"https://localhost:7001/swagger"
)

foreach($u in $urls)
{
    try
    {
        $r=Invoke-WebRequest $u -UseBasicParsing -TimeoutSec 2

        if($r.StatusCode -eq 200)
        {
            Start-Process $u

            Write-Host ""
            Write-Host "Swagger berhasil dibuka." -ForegroundColor Green

            break
        }
    }
    catch{}
}

#---------------------------------------------------------

Section "RUN COMPLETE"

Write-Host ""
Write-Host "===============================================" -ForegroundColor Green
Write-Host "KYVARA API BERHASIL DIJALANKAN" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green

Write-Host ""
Write-Host "Perintah berikutnya:"
Write-Host ""
Write-Host "    .\doctor.ps1"
Write-Host "    .\repair.ps1"
Write-Host "    .\database.ps1"
Write-Host "    .\build.ps1"
Write-Host "    .\run.ps1"
Write-Host ""

