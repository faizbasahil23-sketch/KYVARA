$ErrorActionPreference="Stop"

#====================================================
# KYVARA SELF HEALING ENGINE v2
# CORE ENGINE
#====================================================

$Global:Version="2.0"

$Global:ScriptRoot=$PSScriptRoot

if([string]::IsNullOrWhiteSpace($Global:ScriptRoot))
{
    $Global:ScriptRoot=Split-Path $MyInvocation.MyCommand.Path
}

$Global:Root=Split-Path $Global:ScriptRoot -Parent

$Global:LogFolder="$Global:Root\logs"
$Global:BackupFolder="$Global:Root\backup"
$Global:ReportFolder="$Global:Root\reports"
$Global:TempFolder="$Global:Root\temp"

$Global:LogFile="$Global:LogFolder\selfheal.log"

$Global:ReportFile="$Global:ReportFolder\heal-report.json"

$Global:StartTime=Get-Date

$Global:Health=[ordered]@{

Version=$Global:Version

StartTime=$Global:StartTime

Computer=$env:COMPUTERNAME

User=$env:USERNAME

OS=""

PowerShell=""

DotNet=""

Git=""

SQLite=""

ProjectFound=$false

SolutionFound=$false

BuildSuccess=$false

RestoreSuccess=$false

DatabaseFound=$false

MigrationFound=$false

RepairCount=0

Errors=@()

Warnings=@()

Repairs=@()

Elapsed=""

}

#====================================================
# CREATE FOLDERS
#====================================================

$Folders=@(

$Global:LogFolder,

$Global:BackupFolder,

$Global:ReportFolder,

$Global:TempFolder

)

foreach($folder in $Folders)
{

if(!(Test-Path $folder))
{

New-Item `
ItemType Directory `
Path $folder `
Force | Out-Null

}

}

#====================================================
# LOG
#====================================================

function Write-Log
{

param([string]$Text)

$time=Get-Date -Format "yyyy-MM-dd HH:mm:ss"

"$time  $Text" |

Out-File $Global:LogFile `
Append `
Encoding utf8

}

#====================================================
# COLORS
#====================================================

function Success
{

param($Text)

Write-Host "[ OK ] $Text" `
ForegroundColor Green

Write-Log $Text

}

function Warning
{

param($Text)

Write-Host "[WARN] $Text" `
ForegroundColor Yellow

$Global:Health.Warnings+=$Text

Write-Log $Text

}

function Failure
{

param($Text)

Write-Host "[FAIL] $Text" `
ForegroundColor Red

$Global:Health.Errors+=$Text

Write-Log $Text

}

function Info
{

param($Text)

Write-Host "[INFO] $Text" `
ForegroundColor Cyan

Write-Log $Text

}

#====================================================
# REPAIR COUNTER
#====================================================

function Add-Repair
{

param($Text)

$Global:Health.RepairCount++

$Global:Health.Repairs+=$Text

Write-Log "Repair : $Text"

}

#====================================================
# BANNER
#====================================================

function Show-Banner
{

Clear-Host

Write-Host ""

Write-Host "==========================================================" -ForegroundColor Cyan

Write-Host "          KYVARA SELF HEALING ENGINE v2" -ForegroundColor Yellow

Write-Host "==========================================================" -ForegroundColor Cyan

Write-Host ""

Write-Host "Version : $($Global:Version)"

Write-Host "Root    : $($Global:Root)"

Write-Host ""

}

#====================================================
# SAVE REPORT
#====================================================

function Save-Report
{

$Global:Health.Elapsed=((Get-Date)-$Global:StartTime).ToString()

$Global:Health |

ConvertTo-Json `
Depth 10 |

Out-File `
$Global:ReportFile `
Encoding utf8

}

#====================================================
# FINISH
#====================================================

function Finish
{

Save-Report

Write-Host ""

Write-Host "==========================================================" -ForegroundColor Green

Write-Host "SELF HEAL COMPLETED"

Write-Host "==========================================================" -ForegroundColor Green

Write-Host ""

Write-Host "Report"

Write-Host $Global:ReportFile

Write-Host ""

Write-Host "Log"

Write-Host $Global:LogFile

Write-Host ""

}

Show-Banner

Info "Core Engine Loaded"


#====================================================
# PROJECT SCANNER
#====================================================

Info "Scanning Environment..."

#----------------------------------------------------
# OS
#----------------------------------------------------

try
{
    $os=(Get-CimInstance Win32_OperatingSystem).Caption

    $Global:Health.OS=$os

    Success "OS : $os"
}
catch
{
    Failure "Cannot detect Operating System."
}

#----------------------------------------------------
# PowerShell
#----------------------------------------------------

try
{
    $ps=$PSVersionTable.PSVersion.ToString()

    $Global:Health.PowerShell=$ps

    Success "PowerShell : $ps"
}
catch
{
    Failure "PowerShell not detected."
}

#----------------------------------------------------
# DOTNET
#----------------------------------------------------

try
{
    $dotnet=dotnet --version

    $Global:Health.DotNet=$dotnet

    Success ".NET SDK : $dotnet"
}
catch
{
    Failure ".NET SDK not installed."
}

#----------------------------------------------------
# GIT
#----------------------------------------------------

try
{
    $git=git --version

    $Global:Health.Git=$git

    Success $git
}
catch
{
    Warning "Git not installed."
}

#----------------------------------------------------
# SQLITE
#----------------------------------------------------

$sqlite=$null

$sqlite=Get-ChildItem "C:\" `
Filter sqlite3.exe `
Recurse `
ErrorAction SilentlyContinue |
Select-Object -First 1

if($sqlite)
{
    $Global:Health.SQLite=$sqlite.FullName

    Success "SQLite : $($sqlite.FullName)"
}
else
{
    Warning "sqlite3.exe not found."
}

#----------------------------------------------------
# SOLUTION
#----------------------------------------------------

$solution=

Get-ChildItem $Global:Root `
-Recurse `
-Include *.sln,*.slnx |
Select-Object -First 1

if($solution)
{
    $Global:Health.SolutionFound=$true

    Success "Solution : $($solution.Name)"
}
else
{
    Failure "Solution file not found."
}

#----------------------------------------------------
# PROJECTS
#----------------------------------------------------

$projects=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.csproj

if($projects.Count -gt 0)
{

$Global:Health.ProjectFound=$true

Success "Projects : $($projects.Count)"

foreach($p in $projects)
{

Write-Host "   - $($p.Name)"

}

}
else
{

Failure "No csproj found."

}

#----------------------------------------------------
# APPSETTINGS
#----------------------------------------------------

$app=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter appsettings.json |
Select-Object -First 1

if($app)
{

Success "appsettings.json"

}
else
{

Failure "appsettings.json missing."

}

#----------------------------------------------------
# DATABASE
#----------------------------------------------------

$db=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.db |
Select-Object -First 1

if($db)
{

$Global:Health.DatabaseFound=$true

Success "Database : $($db.Name)"

}
else
{

Warning "Database not found."

}

#----------------------------------------------------
# MIGRATIONS
#----------------------------------------------------

$migration=

Get-ChildItem `
$Global:Root `
-Recurse `
Directory |
Where-Object{

$_.Name -eq "Migrations"

}

if($migration)
{

$Global:Health.MigrationFound=$true

Success "Migration Folder"

}
else
{

Warning "Migration folder missing."

}

#----------------------------------------------------
# PACKAGE COUNT
#----------------------------------------------------

$totalPackages=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.csproj |

ForEach-Object{

Select-String `
Path $_.FullName `
Pattern "<PackageReference"

} |

Measure-Object

Success "NuGet Packages : $($totalPackages.Count)"

#----------------------------------------------------
# SOURCE FILES
#----------------------------------------------------

$cs=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.cs |

Measure-Object

Success "C# Files : $($cs.Count)"

#----------------------------------------------------
# BUILD OUTPUT
#----------------------------------------------------

$dll=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.dll |

Measure-Object

Success "DLL : $($dll.Count)"

#----------------------------------------------------
# REPORT
#----------------------------------------------------

Info "Environment Scan Finished."


#====================================================
# REPAIR ENGINE
#====================================================

Info "Starting Repair Engine..."

#----------------------------------------------------
# EXECUTE
#----------------------------------------------------

function Invoke-Step
{
    param(
        [string]$Name,
        [scriptblock]$Action
    )

    Info $Name

    try
    {
        & $Action

        Success $Name
    }
    catch
    {
        Failure "$Name : $($_.Exception.Message)"
    }
}

#----------------------------------------------------
# CLEAR NUGET CACHE
#----------------------------------------------------

Invoke-Step "NuGet Cache"
{

    dotnet nuget locals all --clear

    Add-Repair "NuGet cache cleared"

}

#----------------------------------------------------
# RESTORE
#----------------------------------------------------

Invoke-Step "Restore Packages"
{

    dotnet restore

    if($LASTEXITCODE -ne 0)
    {
        throw "Restore failed."
    }

    $Global:Health.RestoreSuccess=$true

    Add-Repair "Packages restored"

}

#----------------------------------------------------
# REMOVE BIN
#----------------------------------------------------

Invoke-Step "Remove bin folders"
{

Get-ChildItem `
$Global:Root `
-Recurse `
Directory `
Filter bin |

ForEach-Object{

Remove-Item `
$_.FullName `
Recurse `
Force `
ErrorAction SilentlyContinue

}

Add-Repair "bin removed"

}

#----------------------------------------------------
# REMOVE OBJ
#----------------------------------------------------

Invoke-Step "Remove obj folders"
{

Get-ChildItem `
$Global:Root `
-Recurse `
Directory `
Filter obj |

ForEach-Object{

Remove-Item `
$_.FullName `
Recurse `
Force `
ErrorAction SilentlyContinue

}

Add-Repair "obj removed"

}

#----------------------------------------------------
# BUILD
#----------------------------------------------------

Invoke-Step "Build Solution"
{

dotnet build

if($LASTEXITCODE -ne 0)
{
throw "Build failed."
}

$Global:Health.BuildSuccess=$true

Add-Repair "Solution rebuilt"

}

#----------------------------------------------------
# EF DATABASE UPDATE
#----------------------------------------------------

if($Global:Health.MigrationFound)
{

Invoke-Step "Migration Update"
{

dotnet ef database update

Add-Repair "Database updated"

}

}

#----------------------------------------------------
# VERIFY DATABASE
#----------------------------------------------------

$db=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.db |
Select-Object -First 1

if($db)
{

Success "Database OK"

}
else
{

Warning "Database missing after repair."

}

#----------------------------------------------------
# VERIFY APPSETTINGS
#----------------------------------------------------

$app=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter appsettings.json |
Select-Object -First 1

if($app)
{

Success "Configuration OK"

}
else
{

Failure "appsettings.json missing."

}

#----------------------------------------------------
# VERIFY DLL
#----------------------------------------------------

$dll=

Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.dll

if($dll.Count -gt 0)
{

Success "Compiled DLL : $($dll.Count)"

}
else
{

Failure "No compiled DLL found."

}

#----------------------------------------------------
# REPAIR SUMMARY
#----------------------------------------------------

Info ""

Info "Repair Summary"

Info "Repairs : $($Global:Health.RepairCount)"

Info "Warnings : $($Global:Health.Warnings.Count)"

Info "Errors : $($Global:Health.Errors.Count)"

Info ""


#====================================================
# AUTO RECOVERY ENGINE
#====================================================

Info "Starting Auto Recovery Engine..."

#----------------------------------------------------
# BACKUP
#----------------------------------------------------

function Backup-BeforeRepair
{

$time=Get-Date -Format "yyyyMMdd_HHmmss"

$backup="$Global:BackupFolder\Recovery_$time"

New-Item `
ItemType Directory `
Path $backup `
Force | Out-Null

Get-ChildItem `
$Global:Root `
-Recurse `
-Include *.db,appsettings*.json,*.csproj,*.sln,*.slnx |

ForEach-Object{

$dest=Join-Path $backup $_.Name

Copy-Item `
$_.FullName `
$dest `
Force `
ErrorAction SilentlyContinue

}

Success "Backup created"

Add-Repair "Recovery Backup"

}

#----------------------------------------------------
# BUILD WITH RETRY
#----------------------------------------------------

function Build-WithRetry
{

$success=$false

for($i=1;$i -le 3;$i++)
{

Info "Build Attempt $i"

dotnet build

if($LASTEXITCODE -eq 0)
{

Success "Build Success"

$Global:Health.BuildSuccess=$true

$success=$true

break

}

Warning "Build failed"

Start-Sleep 2

}

if(!$success)
{

Failure "Build failed after 3 attempts."

}

return $success

}

#----------------------------------------------------
# RESTORE WITH RETRY
#----------------------------------------------------

function Restore-WithRetry
{

for($i=1;$i -le 3;$i++)
{

Info "Restore Attempt $i"

dotnet restore

if($LASTEXITCODE -eq 0)
{

Success "Restore Success"

$Global:Health.RestoreSuccess=$true

return $true

}

Start-Sleep 2

}

Failure "Restore failed."

return $false

}

#----------------------------------------------------
# CREATE DATABASE
#----------------------------------------------------

function Ensure-Database
{

$db=Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.db `
-ErrorAction SilentlyContinue |
Select-Object -First 1

if($db)
{

Success "Database Exists"

return

}

Warning "Database Missing"

dotnet ef database update

$db=Get-ChildItem `
$Global:Root `
-Recurse `
Filter *.db `
-ErrorAction SilentlyContinue |
Select-Object -First 1

if($db)
{

Success "Database Created"

Add-Repair "Database Auto Created"

}
else
{

Failure "Database creation failed."

}

}

#----------------------------------------------------
# API
#----------------------------------------------------

function Restart-Api
{

$running=Get-Process dotnet `
ErrorAction SilentlyContinue

if($running)
{

$running | Stop-Process -Force

Start-Sleep 2

}

Start-Process powershell `
"-NoExit -Command cd '$Global:Root'; dotnet run --project .\Kyvara.Api"

Success "API Restarted"

Add-Repair "API Restart"

}

#----------------------------------------------------
# HEALTH SCORE
#----------------------------------------------------

function Calculate-HealthScore
{

$score=100

$score-=($Global:Health.Errors.Count*15)

$score-=($Global:Health.Warnings.Count*5)

if(!$Global:Health.ProjectFound){$score-=20}

if(!$Global:Health.DatabaseFound){$score-=15}

if(!$Global:Health.BuildSuccess){$score-=20}

if(!$Global:Health.RestoreSuccess){$score-=10}

if($score -lt 0)
{

$score=0

}

$Global:Health.HealthScore=$score

Success "Health Score : $score"

}

#----------------------------------------------------
# PUBLISH
#----------------------------------------------------

function Auto-Publish
{

if(!$Global:Health.BuildSuccess)
{

Warning "Publish skipped."

return

}

dotnet publish `
.\Kyvara.Api `
-c Release `
-o "$Global:Root\publish"

if($LASTEXITCODE -eq 0)
{

Success "Publish Success"

Add-Repair "Publish"

}
else
{

Failure "Publish failed."

}

}

#----------------------------------------------------
# MAIN
#----------------------------------------------------

Backup-BeforeRepair

Restore-WithRetry

Build-WithRetry

Ensure-Database

Restart-Api

Auto-Publish

Calculate-HealthScore

Info "Auto Recovery Finished"


#====================================================
# ENTERPRISE REPORT ENGINE
#====================================================

Info "Generating Enterprise Report..."

#----------------------------------------------------
# GIT INFORMATION
#----------------------------------------------------

try
{
    $branch=git branch --show-current
    $commit=git rev-parse --short HEAD
    $status=git status --short

    $Global:Health.GitBranch=$branch
    $Global:Health.GitCommit=$commit
    $Global:Health.GitDirty=($status.Count -gt 0)

    Success "Git Information Collected"
}
catch
{
    Warning "Git repository unavailable."
}

#----------------------------------------------------
# PROJECT STATISTICS
#----------------------------------------------------

$Stats=[ordered]@{}

$Stats.Projects=(Get-ChildItem $Global:Root -Recurse -Filter *.csproj -ErrorAction SilentlyContinue).Count
$Stats.CSharpFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.cs -ErrorAction SilentlyContinue).Count
$Stats.JsonFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.json -ErrorAction SilentlyContinue).Count
$Stats.PowerShellFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.ps1 -ErrorAction SilentlyContinue).Count
$Stats.DllFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.dll -ErrorAction SilentlyContinue).Count
$Stats.Migrations=(Get-ChildItem $Global:Root -Recurse -Filter *.cs -ErrorAction SilentlyContinue |
Select-String "Migration" | Measure-Object).Count

$Global:Health.ProjectStatistics=$Stats

#----------------------------------------------------
# DIRECTORY SIZE
#----------------------------------------------------

try
{
    $bytes=(Get-ChildItem $Global:Root -Recurse -File |
    Measure-Object Length -Sum).Sum

    $Global:Health.ProjectSizeMB=[math]::Round($bytes/1MB,2)
}
catch
{
    $Global:Health.ProjectSizeMB=0
}

#----------------------------------------------------
# PACKAGE REFERENCES
#----------------------------------------------------

$packages=@()

Get-ChildItem $Global:Root -Recurse -Filter *.csproj |
ForEach-Object{

Select-String $_.FullName "<PackageReference" |
ForEach-Object{

$packages+=$_.Line.Trim()

}

}

$Global:Health.PackageCount=$packages.Count

#----------------------------------------------------
# BUILD STATUS
#----------------------------------------------------

$Global:Health.BuildDate=Get-Date

$Global:Health.TotalErrors=$Global:Health.Errors.Count

$Global:Health.TotalWarnings=$Global:Health.Warnings.Count

#----------------------------------------------------
# MARKDOWN REPORT
#----------------------------------------------------

$md=@"

# KYVARA SELF HEAL REPORT

Generated : $(Get-Date)

---

## Health Score

$($Global:Health.HealthScore)

---

## Build

Build Success : $($Global:Health.BuildSuccess)

Restore Success : $($Global:Health.RestoreSuccess)

Database : $($Global:Health.DatabaseFound)

---

## Statistics

Projects : $($Stats.Projects)

C# Files : $($Stats.CSharpFiles)

DLL : $($Stats.DllFiles)

Packages : $($Global:Health.PackageCount)

Project Size : $($Global:Health.ProjectSizeMB) MB

---

## Repairs

$($Global:Health.RepairCount)

---

## Errors

$($Global:Health.Errors.Count)

---

## Warnings

$($Global:Health.Warnings.Count)

"@

$md | Set-Content "$Global:ReportFolder\heal-report.md"

#----------------------------------------------------
# HTML REPORT
#----------------------------------------------------

$html=@"

<html>

<head>

<title>KYVARA Report</title>

<style>

body{

font-family:Segoe UI;

padding:30px;

background:#f7f7f7;

}

table{

border-collapse:collapse;

width:100%;

}

td,th{

border:1px solid #ccc;

padding:8px;

}

th{

background:#222;

color:white;

}

.good{

color:green;

font-weight:bold;

}

.warn{

color:orange;

font-weight:bold;

}

.bad{

color:red;

font-weight:bold;

}

</style>

</head>

<body>

<h1>KYVARA Enterprise Report</h1>

<table>

<tr>

<th>Item</th>

<th>Value</th>

</tr>

<tr>

<td>Health Score</td>

<td>$($Global:Health.HealthScore)</td>

</tr>

<tr>

<td>Projects</td>

<td>$($Stats.Projects)</td>

</tr>

<tr>

<td>C# Files</td>

<td>$($Stats.CSharpFiles)</td>

</tr>

<tr>

<td>Packages</td>

<td>$($Global:Health.PackageCount)</td>

</tr>

<tr>

<td>Repairs</td>

<td>$($Global:Health.RepairCount)</td>

</tr>

<tr>

<td>Errors</td>

<td>$($Global:Health.Errors.Count)</td>

</tr>

<tr>

<td>Warnings</td>

<td>$($Global:Health.Warnings.Count)</td>

</tr>

<tr>

<td>Build</td>

<td>$($Global:Health.BuildSuccess)</td>

</tr>

<tr>

<td>Restore</td>

<td>$($Global:Health.RestoreSuccess)</td>

</tr>

<tr>

<td>Database</td>

<td>$($Global:Health.DatabaseFound)</td>

</tr>

<tr>

<td>Project Size</td>

<td>$($Global:Health.ProjectSizeMB) MB</td>

</tr>

</table>

</body>

</html>

"@

$html | Set-Content "$Global:ReportFolder\heal-report.html"

#----------------------------------------------------
# JSON
#----------------------------------------------------

Save-Report

Success "Markdown Report Generated"

Success "HTML Report Generated"

Success "JSON Report Generated"

Info "Enterprise Report Complete"


#====================================================
# ENTERPRISE REPORT ENGINE
#====================================================

Info "Generating Enterprise Report..."

#----------------------------------------------------
# GIT INFORMATION
#----------------------------------------------------

try
{
    $branch=git branch --show-current
    $commit=git rev-parse --short HEAD
    $status=git status --short

    $Global:Health.GitBranch=$branch
    $Global:Health.GitCommit=$commit
    $Global:Health.GitDirty=($status.Count -gt 0)

    Success "Git Information Collected"
}
catch
{
    Warning "Git repository unavailable."
}

#----------------------------------------------------
# PROJECT STATISTICS
#----------------------------------------------------

$Stats=[ordered]@{}

$Stats.Projects=(Get-ChildItem $Global:Root -Recurse -Filter *.csproj -ErrorAction SilentlyContinue).Count
$Stats.CSharpFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.cs -ErrorAction SilentlyContinue).Count
$Stats.JsonFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.json -ErrorAction SilentlyContinue).Count
$Stats.PowerShellFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.ps1 -ErrorAction SilentlyContinue).Count
$Stats.DllFiles=(Get-ChildItem $Global:Root -Recurse -Filter *.dll -ErrorAction SilentlyContinue).Count
$Stats.Migrations=(Get-ChildItem $Global:Root -Recurse -Filter *.cs -ErrorAction SilentlyContinue |
Select-String "Migration" | Measure-Object).Count

$Global:Health.ProjectStatistics=$Stats

#----------------------------------------------------
# DIRECTORY SIZE
#----------------------------------------------------

try
{
    $bytes=(Get-ChildItem $Global:Root -Recurse -File |
    Measure-Object Length -Sum).Sum

    $Global:Health.ProjectSizeMB=[math]::Round($bytes/1MB,2)
}
catch
{
    $Global:Health.ProjectSizeMB=0
}

#----------------------------------------------------
# PACKAGE REFERENCES
#----------------------------------------------------

$packages=@()

Get-ChildItem $Global:Root -Recurse -Filter *.csproj |
ForEach-Object{

Select-String $_.FullName "<PackageReference" |
ForEach-Object{

$packages+=$_.Line.Trim()

}

}

$Global:Health.PackageCount=$packages.Count

#----------------------------------------------------
# BUILD STATUS
#----------------------------------------------------

$Global:Health.BuildDate=Get-Date

$Global:Health.TotalErrors=$Global:Health.Errors.Count

$Global:Health.TotalWarnings=$Global:Health.Warnings.Count

#----------------------------------------------------
# MARKDOWN REPORT
#----------------------------------------------------

$md=@"

# KYVARA SELF HEAL REPORT

Generated : $(Get-Date)

---

## Health Score

$($Global:Health.HealthScore)

---

## Build

Build Success : $($Global:Health.BuildSuccess)

Restore Success : $($Global:Health.RestoreSuccess)

Database : $($Global:Health.DatabaseFound)

---

## Statistics

Projects : $($Stats.Projects)

C# Files : $($Stats.CSharpFiles)

DLL : $($Stats.DllFiles)

Packages : $($Global:Health.PackageCount)

Project Size : $($Global:Health.ProjectSizeMB) MB

---

## Repairs

$($Global:Health.RepairCount)

---

## Errors

$($Global:Health.Errors.Count)

---

## Warnings

$($Global:Health.Warnings.Count)

"@

$md | Set-Content "$Global:ReportFolder\heal-report.md"

#----------------------------------------------------
# HTML REPORT
#----------------------------------------------------

$html=@"

<html>

<head>

<title>KYVARA Report</title>

<style>

body{

font-family:Segoe UI;

padding:30px;

background:#f7f7f7;

}

table{

border-collapse:collapse;

width:100%;

}

td,th{

border:1px solid #ccc;

padding:8px;

}

th{

background:#222;

color:white;

}

.good{

color:green;

font-weight:bold;

}

.warn{

color:orange;

font-weight:bold;

}

.bad{

color:red;

font-weight:bold;

}

</style>

</head>

<body>

<h1>KYVARA Enterprise Report</h1>

<table>

<tr>

<th>Item</th>

<th>Value</th>

</tr>

<tr>

<td>Health Score</td>

<td>$($Global:Health.HealthScore)</td>

</tr>

<tr>

<td>Projects</td>

<td>$($Stats.Projects)</td>

</tr>

<tr>

<td>C# Files</td>

<td>$($Stats.CSharpFiles)</td>

</tr>

<tr>

<td>Packages</td>

<td>$($Global:Health.PackageCount)</td>

</tr>

<tr>

<td>Repairs</td>

<td>$($Global:Health.RepairCount)</td>

</tr>

<tr>

<td>Errors</td>

<td>$($Global:Health.Errors.Count)</td>

</tr>

<tr>

<td>Warnings</td>

<td>$($Global:Health.Warnings.Count)</td>

</tr>

<tr>

<td>Build</td>

<td>$($Global:Health.BuildSuccess)</td>

</tr>

<tr>

<td>Restore</td>

<td>$($Global:Health.RestoreSuccess)</td>

</tr>

<tr>

<td>Database</td>

<td>$($Global:Health.DatabaseFound)</td>

</tr>

<tr>

<td>Project Size</td>

<td>$($Global:Health.ProjectSizeMB) MB</td>

</tr>

</table>

</body>

</html>

"@

$html | Set-Content "$Global:ReportFolder\heal-report.html"

#----------------------------------------------------
# JSON
#----------------------------------------------------

Save-Report

Success "Markdown Report Generated"

Success "HTML Report Generated"

Success "JSON Report Generated"

Info "Enterprise Report Complete"

