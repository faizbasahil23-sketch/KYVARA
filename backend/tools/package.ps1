. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA PACKAGE MANAGER"

Write-Log "Package Manager Started"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "SOLUTION" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host $Global:SOLUTION

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "INSTALLED PACKAGES" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

dotnet list "$Global:SOLUTION" package

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "OUTDATED PACKAGES" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

dotnet list "$Global:SOLUTION" package --outdated

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "VULNERABILITY SCAN" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

dotnet list "$Global:SOLUTION" package --vulnerable

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "TRANSITIVE DEPENDENCIES" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

dotnet list "$Global:SOLUTION" package --include-transitive

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "RESTORE TEST" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

dotnet restore "$Global:SOLUTION"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "PACKAGE STATISTICS" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$Projects=Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.csproj

$PackageCount=0

foreach($Project in $Projects)
{
    try
    {
        [xml]$xml=Get-Content $Project.FullName
        $PackageCount+=$xml.Project.ItemGroup.PackageReference.Count
    }
    catch{}
}

Write-Host "Projects :" $Projects.Count
Write-Host "Packages :" $PackageCount

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "PACKAGE REPORT" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$ReportFolder="$Global:KYVARA_ROOT\reports"

if(!(Test-Path $ReportFolder))
{
    New-Item $ReportFolder -ItemType Directory | Out-Null
}

$Report="$ReportFolder\PackageReport.txt"

"KYVARA PACKAGE REPORT" | Out-File $Report
"" | Out-File $Report -Append

dotnet list "$Global:SOLUTION" package | Out-File $Report -Append

"" | Out-File $Report -Append

dotnet list "$Global:SOLUTION" package --outdated | Out-File $Report -Append

"" | Out-File $Report -Append

dotnet list "$Global:SOLUTION" package --vulnerable | Out-File $Report -Append

Write-Host ""
Write-Host "Report :" $Report -ForegroundColor Green

Write-Host ""
Write-Host "==========================================" -ForegroundColor Green
Write-Host "PACKAGE MANAGER FINISHED" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green

Write-Log "Package Manager Finished"
