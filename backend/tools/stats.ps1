. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA PROJECT STATISTICS"

Write-Log "Stats Started"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "COLLECTING PROJECT DATA..." -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan

$Projects=Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.csproj

$Solutions=Get-ChildItem $Global:KYVARA_ROOT -Recurse -Include *.sln,*.slnx

$CsFiles=Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.cs

$Classes=(Select-String -Path $CsFiles.FullName -Pattern "class ").Count

$Interfaces=(Select-String -Path $CsFiles.FullName -Pattern "interface ").Count

$Records=(Select-String -Path $CsFiles.FullName -Pattern "record ").Count

$Enums=(Select-String -Path $CsFiles.FullName -Pattern "enum ").Count

$Controllers=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Controller.cs).Count

$Endpoints=(Select-String -Path $CsFiles.FullName -Pattern "\[Http").Count

$Migrations=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Include *Migration*.cs).Count

$Tests=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Include *Test*.cs).Count

$Packages=0

foreach($p in $Projects)
{
    try{

        [xml]$xml=Get-Content $p.FullName

        $Packages+=$xml.Project.ItemGroup.PackageReference.Count

    }catch{}
}

$Folders=(Get-ChildItem $Global:KYVARA_ROOT -Directory -Recurse).Count

$Files=(Get-ChildItem $Global:KYVARA_ROOT -File -Recurse).Count

$Size=((Get-ChildItem $Global:KYVARA_ROOT -File -Recurse |
Measure Length -Sum).Sum/1MB)

$Health=100

if($Controllers -lt 3){$Health-=5}
if($Packages -gt 100){$Health-=5}
if($Tests -eq 0){$Health-=10}
if($Migrations -eq 0){$Health-=5}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Green
Write-Host "KYVARA STATISTICS" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green

Write-Host ""
Write-Host ("Project Root".PadRight(25))+": "+$Global:KYVARA_ROOT
Write-Host ("Solutions".PadRight(25))+": "+$Solutions.Count
Write-Host ("Projects".PadRight(25))+": "+$Projects.Count
Write-Host ("Folders".PadRight(25))+": "+$Folders
Write-Host ("Files".PadRight(25))+": "+$Files
Write-Host ("C# Files".PadRight(25))+": "+$CsFiles.Count
Write-Host ("Classes".PadRight(25))+": "+$Classes
Write-Host ("Interfaces".PadRight(25))+": "+$Interfaces
Write-Host ("Records".PadRight(25))+": "+$Records
Write-Host ("Enums".PadRight(25))+": "+$Enums
Write-Host ("Controllers".PadRight(25))+": "+$Controllers
Write-Host ("API Endpoints".PadRight(25))+": "+$Endpoints
Write-Host ("EF Migrations".PadRight(25))+": "+$Migrations
Write-Host ("Tests".PadRight(25))+": "+$Tests
Write-Host ("NuGet Packages".PadRight(25))+": "+$Packages
Write-Host ("Project Size (MB)".PadRight(25))+": "+([math]::Round($Size,2))
Write-Host ("Health Score".PadRight(25))+": "+$Health+"/100"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "TOP 10 LARGEST FILES" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan

Get-ChildItem $Global:KYVARA_ROOT -File -Recurse |
Sort Length -Descending |
Select -First 10 Name,
@{N="Size(MB)";E={[math]::Round($_.Length/1MB,2)}} |
Format-Table -AutoSize

$ReportFolder="$Global:KYVARA_ROOT\reports"

if(!(Test-Path $ReportFolder))
{
    New-Item $ReportFolder -ItemType Directory | Out-Null
}

$Report="$ReportFolder\ProjectStatistics.txt"

@"

KYVARA PROJECT STATISTICS

Generated : $(Get-Date)

Projects      : $($Projects.Count)
Controllers   : $Controllers
Endpoints     : $Endpoints
Classes        : $Classes
Interfaces     : $Interfaces
Records        : $Records
Enums          : $Enums
Packages       : $Packages
Tests          : $Tests
Health Score   : $Health

"@ | Out-File $Report -Encoding UTF8

Write-Host ""
Write-Host "Report Saved :" $Report -ForegroundColor Green

Write-Host ""
Write-Host "STATISTICS COMPLETED" -ForegroundColor Green

Write-Log "Stats Finished"

