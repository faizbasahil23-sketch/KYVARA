Clear-Host

$ErrorActionPreference="Continue"

function Section($title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Cyan
    Write-Host "===================================================" -ForegroundColor Yellow
}

Section "KYVARA BACKUP v1.0"

$Root = Get-Location

$TimeStamp = Get-Date -Format "yyyyMMdd_HHmmss"

$BackupRoot = Join-Path $Root "Backups"

$BackupFolder = Join-Path $BackupRoot $TimeStamp

#-------------------------------------------------------

Section "CREATE BACKUP FOLDER"

if(!(Test-Path $BackupRoot))
{
    New-Item `
    -ItemType Directory `
    -Path $BackupRoot | Out-Null
}

New-Item `
-ItemType Directory `
-Path $BackupFolder | Out-Null

Write-Host $BackupFolder

#-------------------------------------------------------

Section "BACKUP DATABASE"

Get-ChildItem -Recurse *.db -ErrorAction SilentlyContinue |
ForEach-Object{

    $dest = Join-Path $BackupFolder $_.Name

    Copy-Item `
    $_.FullName `
    $dest `
    -Force

    Write-Host $_.Name
}

#-------------------------------------------------------

Section "BACKUP APPSETTINGS"

Get-ChildItem `
-Recurse `
appsettings*.json `
-ErrorAction SilentlyContinue |
ForEach-Object{

    Copy-Item `
    $_.FullName `
    (Join-Path $BackupFolder $_.Name) `
    -Force

    Write-Host $_.Name
}

#-------------------------------------------------------

Section "BACKUP SOLUTION"

Get-ChildItem *.sln* |
ForEach-Object{

    Copy-Item `
    $_.FullName `
    (Join-Path $BackupFolder $_.Name) `
    -Force

    Write-Host $_.Name
}

#-------------------------------------------------------

Section "BACKUP PROJECT FILES"

Get-ChildItem `
-Recurse `
*.csproj |
ForEach-Object{

    $dest = Join-Path $BackupFolder $_.Name

    Copy-Item `
    $_.FullName `
    $dest `
    -Force
}

#-------------------------------------------------------

Section "EXPORT PACKAGE LIST"

dotnet list package |
Out-File `
(FileName = (Join-Path $BackupFolder "packages.txt"))

#-------------------------------------------------------

Section "EXPORT SDK"

dotnet --info |
Out-File `
(FileName = (Join-Path $BackupFolder "dotnet-info.txt"))

#-------------------------------------------------------

Section "EXPORT GIT STATUS"

git status |
Out-File `
(FileName = (Join-Path $BackupFolder "git-status.txt"))

#-------------------------------------------------------

Section "CREATE PROJECT ZIP"

$Zip = Join-Path $BackupRoot ("KYVARA_" + $TimeStamp + ".zip")

Compress-Archive `
-Path .\* `
-DestinationPath $Zip `
-CompressionLevel Optimal `
-Force

Write-Host ""
Write-Host $Zip -ForegroundColor Green

#-------------------------------------------------------

Section "VERIFY"

Get-ChildItem $BackupRoot

#-------------------------------------------------------

Section "FINISHED"

Write-Host ""
Write-Host "===============================================" -ForegroundColor Green
Write-Host "BACKUP BERHASIL DIBUAT" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green

Write-Host ""
Write-Host "Lokasi Backup:"
Write-Host $BackupFolder
Write-Host ""
Write-Host "ZIP:"
Write-Host $Zip

