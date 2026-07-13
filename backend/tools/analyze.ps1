. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA AI PROJECT ANALYZER"

Write-Log "Analyze Started"

$Score=100
$Warnings=@()
$Errors=@()

function Info($m){
    Write-Host "[INFO ] $m" -ForegroundColor Cyan
}

function Warn($m){
    Write-Host "[WARN ] $m" -ForegroundColor Yellow
    $script:Warnings+=$m
    $script:Score-=2
}

function ErrorMsg($m){
    Write-Host "[ERROR] $m" -ForegroundColor Red
    $script:Errors+=$m
    $script:Score-=5
}

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "STRUCTURE ANALYSIS" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

$Required=@(
"Kyvara.Api",
"Kyvara.Application",
"Kyvara.Domain",
"Kyvara.Infrastructure",
"tools"
)

foreach($r in $Required)
{
    if(Test-Path "$Global:KYVARA_ROOT\$r")
    {
        Info "$r"
    }
    else
    {
        ErrorMsg "$r missing"
    }
}

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "DDD ANALYSIS" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

$Entities=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Entity.cs).Count
$Aggregates=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Aggregate*.cs).Count
$Repositories=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Repository*.cs).Count
$Specifications=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Specification*.cs).Count
$ValueObjects=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *ValueObject*.cs).Count

Write-Host "Entities       : $Entities"
Write-Host "Aggregates     : $Aggregates"
Write-Host "Repositories   : $Repositories"
Write-Host "Specifications : $Specifications"
Write-Host "ValueObjects   : $ValueObjects"

if($Repositories -eq 0){Warn "Repository Pattern not found"}
if($ValueObjects -eq 0){Warn "Value Objects not found"}

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "CQRS ANALYSIS" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

$Commands=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Command.cs).Count
$Queries=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Query.cs).Count
$Handlers=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Handler.cs).Count

Write-Host "Commands : $Commands"
Write-Host "Queries  : $Queries"
Write-Host "Handlers : $Handlers"

if($Commands -eq 0){Warn "Commands missing"}
if($Queries -eq 0){Warn "Queries missing"}

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "API ANALYSIS" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

$Controllers=(Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *Controller.cs).Count
$Endpoints=(Select-String -Path (Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.cs).FullName -Pattern "\[Http").Count

Write-Host "Controllers : $Controllers"
Write-Host "Endpoints   : $Endpoints"

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "DATABASE" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

if(Test-Path $Global:DATABASE)
{
    Info "Database found"
}
else
{
    Warn "Database missing"
}

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "SECURITY" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

dotnet list "$Global:SOLUTION" package --vulnerable

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "PACKAGE" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

dotnet list "$Global:SOLUTION" package --outdated

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "CODE METRIC" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

$Classes=(Select-String -Path (Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.cs).FullName -Pattern "class ").Count
$Interfaces=(Select-String -Path (Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.cs).FullName -Pattern "interface ").Count
$Records=(Select-String -Path (Get-ChildItem $Global:KYVARA_ROOT -Recurse -Filter *.cs).FullName -Pattern "record ").Count

Write-Host "Classes    : $Classes"
Write-Host "Interfaces : $Interfaces"
Write-Host "Records    : $Records"

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "AI HEALTH SCORE" -ForegroundColor Yellow
Write-Host "==============================" -ForegroundColor Cyan

if($Score -lt 0){$Score=0}

Write-Host ""
Write-Host "Health Score : $Score /100"

Write-Host ""
Write-Host "Warnings     :" $Warnings.Count
Write-Host "Errors       :" $Errors.Count

if($Warnings.Count -gt 0)
{
    Write-Host ""
    Write-Host "Warnings"
    $Warnings|Sort-Object -Unique
}

if($Errors.Count -gt 0)
{
    Write-Host ""
    Write-Host "Errors"
    $Errors|Sort-Object -Unique
}

$ReportFolder="$Global:KYVARA_ROOT\reports"

if(!(Test-Path $ReportFolder))
{
    New-Item $ReportFolder -ItemType Directory | Out-Null
}

$Report="$ReportFolder\AI_Project_Report.txt"

@"

KYVARA AI PROJECT REPORT

Generated : $(Get-Date)

Health Score : $Score

Controllers : $Controllers
Endpoints   : $Endpoints
Commands    : $Commands
Queries     : $Queries
Handlers    : $Handlers
Entities    : $Entities

Warnings : $($Warnings.Count)
Errors   : $($Errors.Count)

"@ | Out-File $Report -Encoding UTF8

Write-Host ""
Write-Host "Report :" $Report -ForegroundColor Green

Write-Host ""
Write-Host "AI ANALYSIS FINISHED" -ForegroundColor Green

Write-Log "Analyze Finished"

