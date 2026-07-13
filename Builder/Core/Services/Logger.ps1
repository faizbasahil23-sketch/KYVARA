function Write-KyvaraLog
{
    param(
        [string]$Message,
        [string]$Level="INFO"
    )

    $time=Get-Date -Format "yyyy-MM-dd HH:mm:ss"

    Write-Host "[$time][$Level] $Message"

    $logFolder="C:\KYVARA\Builder\Logs"

    if(!(Test-Path $logFolder))
    {
        New-Item $logFolder -ItemType Directory -Force | Out-Null
    }

    Add-Content `
        "$logFolder\Builder.log" `
        "[$time][$Level] $Message"
}
