
$ErrorActionPreference="Stop"

$Global:KyvaraVersion="5.0"

$Global:Root=Split-Path $PSScriptRoot -Parent

$Global:LogFolder="$Global:Root\logs"

$Global:ReportFolder="$Global:Root\reports"

$Global:BackupFolder="$Global:Root\backup"

$Global:PublishFolder="$Global:Root\publish"

$Global:PluginFolder="$Global:Root\plugins"

$Global:TempFolder="$Global:Root\temp"

$folders=@(
$Global:LogFolder,
$Global:ReportFolder,
$Global:BackupFolder,
$Global:PublishFolder,
$Global:PluginFolder,
$Global:TempFolder
)

foreach($f in $folders)
{
if(!(Test-Path $f))
{
New-Item $f -ItemType Directory | Out-Null
}
}

function Banner{

Clear-Host

Write-Host ""
Write-Host "===================================================" -ForegroundColor Cyan
Write-Host "                 KYVARA CLI v5" -ForegroundColor Yellow
Write-Host "===================================================" -ForegroundColor Cyan
Write-Host ""

}

function Log($Text){

$file="$Global:LogFolder\kyvara.log"

(Get-Date -Format "yyyy-MM-dd HH:mm:ss")+"  "+$Text |
Out-File $file -Append

}

function Success($msg){

Write-Host "[ OK ] $msg" -ForegroundColor Green

}

function ErrorMsg($msg){

Write-Host "[FAIL] $msg" -ForegroundColor Red

}

function Warn($msg){

Write-Host "[WARN] $msg" -ForegroundColor Yellow

}

function Info($msg){

Write-Host "[INFO] $msg" -ForegroundColor Cyan

}

function Timer{

return [System.Diagnostics.Stopwatch]::StartNew()

}

function Finish($timer){

$timer.Stop()

Write-Host ""

Write-Host "Execution Time :" $timer.Elapsed

}

