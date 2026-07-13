. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA ENTERPRISE WATCH"

Write-Log "Enterprise Watch Started"

$Api="$Global:KYVARA_ROOT\Kyvara.Api"

$LastBuild=Get-Date

$Watcher=New-Object System.IO.FileSystemWatcher

$Watcher.Path=$Global:KYVARA_ROOT
$Watcher.IncludeSubdirectories=$true
$Watcher.EnableRaisingEvents=$true

$Watcher.Filter="*.*"

Register-ObjectEvent $Watcher Changed -Action {

Write-Host ""

Write-Host "Change :" $Event.SourceEventArgs.FullPath -ForegroundColor Yellow

}

Register-ObjectEvent $Watcher Created -Action {

Write-Host ""

Write-Host "Created :" $Event.SourceEventArgs.FullPath -ForegroundColor Green

}

Register-ObjectEvent $Watcher Deleted -Action {

Write-Host ""

Write-Host "Deleted :" $Event.SourceEventArgs.FullPath -ForegroundColor Red

}

Register-ObjectEvent $Watcher Renamed -Action {

Write-Host ""

Write-Host "Renamed :" $Event.SourceEventArgs.FullPath -ForegroundColor Cyan

}

Push-Location $Api

Start-Job{

dotnet watch run

}|Out-Null

Pop-Location

while($true)
{

Clear-Host

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "KYVARA LIVE DEVELOPMENT" -ForegroundColor Yellow
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Watching :" $Global:KYVARA_ROOT
Write-Host ""

Write-Host "Time :" (Get-Date)

Write-Host ""

Write-Host "Press CTRL+C to stop."

Start-Sleep 3

}

Write-Log "Enterprise Watch Finished"
