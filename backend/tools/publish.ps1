Clear-Host

$ErrorActionPreference = "Continue"

function Section($title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Cyan
    Write-Host "===================================================" -ForegroundColor Yellow
}

Section "KYVARA PUBLISH v1.0"

$Root = Get-Location

$PublishRoot = Join-Path $Root "Publish"

$TimeStamp = Get-Date -Format "yyyyMMdd_HHmmss"

$Output = Join-Path $PublishRoot $TimeStamp

if(!(Test-Path $PublishRoot))
{
    New-Item `
    -ItemType Directory `
    -Path $PublishRoot | Out-Null
}

Section "RESTORE"

dotnet restore

if($LASTEXITCODE -ne 0)
{
    Write-Host "Restore gagal." -ForegroundColor Red
    exit
}

Section "BUILD RELEASE"

dotnet build `
-c Release `
--no-restore

if($LASTEXITCODE -ne 0)
{
    Write-Host "Build gagal." -ForegroundColor Red
    exit
}

Section "DATABASE UPDATE"

dotnet ef database update `
--project .\Kyvara.Infrastructure `
--startup-project .\Kyvara.Api

Section "PUBLISH WIN-X64"

$WinOutput = Join-Path $Output "win-x64"

dotnet publish `
.\Kyvara.Api `
-c Release `
-r win-x64 `
--self-contained false `
-o $WinOutput

Section "PUBLISH LINUX-X64"

$LinuxOutput = Join-Path $Output "linux-x64"

dotnet publish `
.\Kyvara.Api `
-c Release `
-r linux-x64 `
--self-contained false `
-o $LinuxOutput

Section "PUBLISH LINUX ARM64"

$LinuxArmOutput = Join-Path $Output "linux-arm64"

dotnet publish `
.\Kyvara.Api `
-c Release `
-r linux-arm64 `
--self-contained false `
-o $LinuxArmOutput

Section "PUBLISH WINDOWS ARM64"

$WinArmOutput = Join-Path $Output "win-arm64"

dotnet publish `
.\Kyvara.Api `
-c Release `
-r win-arm64 `
--self-contained false `
-o $WinArmOutput

Section "CREATE ZIP"

Compress-Archive `
-Path $Output\* `
-DestinationPath (Join-Path $PublishRoot ("KYVARA_Publish_"+$TimeStamp+".zip")) `
-CompressionLevel Optimal `
-Force

Section "VERIFY"

Get-ChildItem $Output -Recurse

Section "SUMMARY"

Write-Host ""
Write-Host "===============================================" -ForegroundColor Green
Write-Host "KYVARA BERHASIL DIPUBLISH" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green

Write-Host ""
Write-Host "Output Publish :" -ForegroundColor Yellow
Write-Host $Output

Write-Host ""
Write-Host "ZIP :" -ForegroundColor Yellow
Write-Host (Join-Path $PublishRoot ("KYVARA_Publish_"+$TimeStamp+".zip"))

Write-Host ""

Write-Host "Langkah berikutnya:"
Write-Host ""
Write-Host "    .\doctor.ps1"
Write-Host "    .\repair.ps1"
Write-Host "    .\database.ps1"
Write-Host "    .\build.ps1"
Write-Host "    .\run.ps1"
Write-Host "    .\clean.ps1"
Write-Host "    .\backup.ps1"
Write-Host "    .\restore.ps1"
Write-Host "    .\publish.ps1"

