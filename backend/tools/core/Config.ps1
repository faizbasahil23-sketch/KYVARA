$ConfigFile="C:\KYVARA\backend\kyvara.json"

if(!(Test-Path $ConfigFile))
{
    throw "kyvara.json not found."
}

$Global:Config=Get-Content $ConfigFile -Raw | ConvertFrom-Json

$Global:KYVARA_ROOT=$Global:Config.Root
$Global:SOLUTION=$Global:Config.Solution
$Global:TOOLS=$Global:Config.Tools
$Global:BACKUP=$Global:Config.Backup
$Global:PUBLISH=$Global:Config.Publish
$Global:LOGS=$Global:Config.Logs
$Global:DATABASE=$Global:Config.Database
