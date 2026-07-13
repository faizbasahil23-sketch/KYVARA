. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA DEPLOYMENT ENGINE"

Write-Log "Deploy Started"

$PublishRoot="$Global:KYVARA_ROOT\publish"

if(!(Test-Path $PublishRoot))
{
    New-Item $PublishRoot -ItemType Directory | Out-Null
}

$Version=Get-Date -Format "yyyy.MM.dd.HHmmss"

$ReleaseFolder=Join-Path $PublishRoot $Version

New-Item $ReleaseFolder -ItemType Directory | Out-Null

Write-Host ""
Write-Host "Release Version :" $Version -ForegroundColor Green

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "BUILD RELEASE" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Cyan

dotnet publish `
"$Global:SOLUTION" `
-c Release `
-o "$ReleaseFolder"

if($LASTEXITCODE -ne 0)
{
    throw "Publish failed."
}

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "CREATE MANIFEST" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Cyan

$Manifest=@{
Version=$Version
Created=(Get-Date)
Machine=$env:COMPUTERNAME
User=$env:USERNAME
Dotnet=(dotnet --version)
Solution=$Global:SOLUTION
}

$Manifest |
ConvertTo-Json -Depth 5 |
Out-File "$ReleaseFolder\manifest.json"

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "ZIP PACKAGE" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Cyan

Compress-Archive `
-Path "$ReleaseFolder\*" `
-DestinationPath "$PublishRoot\KYVARA-$Version.zip" `
-Force

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "CHECKSUM" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Cyan

Get-FileHash `
"$PublishRoot\KYVARA-$Version.zip" `
-Algorithm SHA256 |
Format-List

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "DEPLOY HISTORY" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Cyan

Get-ChildItem $PublishRoot -Directory |
Sort-Object CreationTime -Descending |
Select-Object Name,CreationTime

Write-Host ""
Write-Host "======================================" -ForegroundColor Green
Write-Host "DEPLOY SUCCESS" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green

Write-Log "Deploy Finished"
