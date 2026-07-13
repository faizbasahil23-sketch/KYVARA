$ErrorActionPreference="Stop"

. "$PSScriptRoot\Config.ps1"
. "$PSScriptRoot\Logger.ps1"
. "$PSScriptRoot\Banner.ps1"
. "$PSScriptRoot\Dotnet.ps1"

foreach($Folder in @(
$Global:TOOLS,
$Global:BACKUP,
$Global:PUBLISH,
$Global:LOGS))
{
    if(!(Test-Path $Folder))
    {
        New-Item $Folder -ItemType Directory | Out-Null
    }
}

Write-Log "Core Loaded"
