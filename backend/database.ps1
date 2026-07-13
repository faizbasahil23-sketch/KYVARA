Clear-Host

$ErrorActionPreference="Continue"

function Section($title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Cyan
    Write-Host "===================================================" -ForegroundColor Yellow
}

Section "KYVARA DATABASE TOOL v1.0"

$db = Get-ChildItem -Recurse *.db | Select-Object -First 1

if($null -eq $db)
{
    Write-Host ""
    Write-Host "Database tidak ditemukan." -ForegroundColor Red
    exit
}

Write-Host ""
Write-Host "Database :" $db.FullName -ForegroundColor Green

Section "DATABASE INFO"

dotnet ef dbcontext info `
--project .\Kyvara.Infrastructure `
--startup-project .\Kyvara.Api

Section "MIGRATIONS"

dotnet ef migrations list `
--project .\Kyvara.Infrastructure `
--startup-project .\Kyvara.Api

Section "UPDATE DATABASE"

dotnet ef database update `
--project .\Kyvara.Infrastructure `
--startup-project .\Kyvara.Api

Section "DATABASE FILE"

Get-ChildItem -Recurse *.db

Section "CHECK USERS"

Get-ChildItem -Recurse *User*.cs

Section "CHECK PROFILE"

Get-ChildItem -Recurse *Profile*.cs

Section "CHECK CONTROLLERS"

Get-ChildItem .\Kyvara.Api\Controllers -Recurse

Section "CHECK REPOSITORIES"

Get-ChildItem .\Kyvara.Infrastructure\Repositories

Section "CHECK CONFIGURATION"

Get-ChildItem .\Kyvara.Infrastructure\Configurations -Recurse

Section "CHECK PERSISTENCE"

Get-ChildItem .\Kyvara.Infrastructure\Persistence -Recurse

Section "CHECK MIGRATION FILES"

Get-ChildItem .\Kyvara.Infrastructure\Persistence\Migrations

Section "DATABASE TOOL FINISHED"

Write-Host ""
Write-Host "DATABASE CHECK SELESAI." -ForegroundColor Green

