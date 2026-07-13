Clear-Host

$ErrorActionPreference="Continue"

function Section($title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Cyan
    Write-Host "===================================================" -ForegroundColor Yellow
}

Section "KYVARA CLEAN v1.0"

Write-Host ""
Write-Host "Folder :" (Get-Location)

#-------------------------------------------------------

Section "DELETE BIN"

Get-ChildItem -Recurse -Directory -Filter bin -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
}

#-------------------------------------------------------

Section "DELETE OBJ"

Get-ChildItem -Recurse -Directory -Filter obj -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
}

#-------------------------------------------------------

Section "DELETE VS"

Get-ChildItem -Recurse -Directory -Filter .vs -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
}

#-------------------------------------------------------

Section "DELETE COMPILED MODELS"

Get-ChildItem -Recurse -Directory -Filter CompiledModels -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
}

#-------------------------------------------------------

Section "DELETE LOG FILES"

Get-ChildItem -Recurse *.log -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
}

#-------------------------------------------------------

Section "DELETE TEMP FILES"

Get-ChildItem -Recurse *.tmp -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
}

Get-ChildItem -Recurse *.temp -ErrorAction SilentlyContinue |
ForEach-Object{
    Write-Host $_.FullName
    Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
}

#-------------------------------------------------------

Section "CLEAR NUGET CACHE"

dotnet nuget locals all --clear

#-------------------------------------------------------

Section "CLEAR HTTP CACHE"

dotnet nuget locals http-cache --clear

#-------------------------------------------------------

Section "CLEAR TEMP"

$env:TEMP

#-------------------------------------------------------

Section "VERIFY PROJECT"

Get-ChildItem -Recurse *.csproj

#-------------------------------------------------------

Section "VERIFY DATABASE"

Get-ChildItem -Recurse *.db

#-------------------------------------------------------

Section "VERIFY SOLUTION"

Get-ChildItem *.sln*
Get-ChildItem *.slnx

#-------------------------------------------------------

Section "SUMMARY"

Write-Host ""
Write-Host "Project berhasil dibersihkan." -ForegroundColor Green
Write-Host ""
Write-Host "Langkah berikutnya:"
Write-Host ""
Write-Host "   .\repair.ps1"
Write-Host "   .\build.ps1"
Write-Host "   .\run.ps1"
Write-Host ""

