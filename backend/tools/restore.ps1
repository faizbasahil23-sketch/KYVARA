Clear-Host

$ErrorActionPreference = "Continue"

function Section($title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Cyan
    Write-Host "===================================================" -ForegroundColor Yellow
}

Section "KYVARA RESTORE v1.0"

$Root = Get-Location
$BackupRoot = Join-Path $Root "Backups"

if(!(Test-Path $BackupRoot))
{
    Write-Host ""
    Write-Host "Folder Backups tidak ditemukan." -ForegroundColor Red
    exit
}

Section "AVAILABLE BACKUPS"

$Folders = Get-ChildItem $BackupRoot -Directory |
Sort-Object LastWriteTime -Descending

if($Folders.Count -eq 0)
{
    Write-Host ""
    Write-Host "Tidak ada backup." -ForegroundColor Red
    exit
}

$index = 1

foreach($folder in $Folders)
{
    Write-Host "$index. $($folder.Name)"
    $index++
}

Write-Host ""
$choice = Read-Host "Pilih nomor backup"

if(-not ($choice -match '^\d+$'))
{
    Write-Host "Input tidak valid."
    exit
}

$Selected = $Folders[[int]$choice-1]

if($null -eq $Selected)
{
    Write-Host "Backup tidak ditemukan."
    exit
}

Section "BACKUP SELECTED"

Write-Host $Selected.FullName -ForegroundColor Green

#-------------------------------------------------------------

Section "RESTORE DATABASE"

Get-ChildItem $Selected.FullName *.db -ErrorAction SilentlyContinue |
ForEach-Object{

    $Target = Join-Path $Root "Kyvara.Api"

    Copy-Item `
    $_.FullName `
    $Target `
    -Force

    Write-Host $_.Name
}

#-------------------------------------------------------------

Section "RESTORE APPSETTINGS"

Get-ChildItem $Selected.FullName appsettings*.json -ErrorAction SilentlyContinue |
ForEach-Object{

    Copy-Item `
    $_.FullName `
    ".\Kyvara.Api" `
    -Force

    Write-Host $_.Name
}

#-------------------------------------------------------------

Section "RESTORE SOLUTION"

Get-ChildItem $Selected.FullName *.sln* -ErrorAction SilentlyContinue |
ForEach-Object{

    Copy-Item `
    $_.FullName `
    "." `
    -Force

    Write-Host $_.Name
}

#-------------------------------------------------------------

Section "RESTORE CSPROJ"

Get-ChildItem $Selected.FullName *.csproj -ErrorAction SilentlyContinue |
ForEach-Object{

    $name = $_.Name

    $target = Get-ChildItem -Recurse $name |
    Select-Object -First 1

    if($target)
    {
        Copy-Item `
        $_.FullName `
        $target.FullName `
        -Force

        Write-Host $name
    }
}

#-------------------------------------------------------------

Section "RESTORE COMPLETE"

Write-Host ""
Write-Host "Restore selesai." -ForegroundColor Green

#-------------------------------------------------------------

Section "RESTORE"

dotnet restore

#-------------------------------------------------------------

Section "BUILD"

dotnet build

#-------------------------------------------------------------

Section "DATABASE UPDATE"

dotnet ef database update `
--project .\Kyvara.Infrastructure `
--startup-project .\Kyvara.Api

#-------------------------------------------------------------

Section "VERIFY"

Get-ChildItem -Recurse *.db

#-------------------------------------------------------------

Section "SUCCESS"

Write-Host ""
Write-Host "===============================================" -ForegroundColor Green
Write-Host "KYVARA BERHASIL DIRESTORE" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green

Write-Host ""
Write-Host "Langkah berikutnya:"
Write-Host ""
Write-Host "    .\doctor.ps1"
Write-Host "    .\build.ps1"
Write-Host "    .\run.ps1"
Write-Host ""

