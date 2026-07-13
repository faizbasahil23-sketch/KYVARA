. "$PSScriptRoot\core\Core.ps1"

param(
    [string]$Name="Sample"
)

Show-Banner "NEW MODULE"

$Module="$Global:KYVARA_ROOT\$Name"

if(Test-Path $Module)
{
    Write-Host "Module already exists."
    exit
}

$Folders=@(
"Application",
"Domain",
"Infrastructure",
"Api",
"Tests"
)

foreach($Folder in $Folders)
{
    New-Item "$Module\$Folder" -ItemType Directory -Force | Out-Null
}

Write-Host ""
Write-Host "Module Created : $Module" -ForegroundColor Green
