$Global:LOGFILE="$Global:LOGS\kyvara.log"

function Write-Log($Text)
{
    if(!(Test-Path $Global:LOGS))
    {
        New-Item $Global:LOGS -ItemType Directory | Out-Null
    }

    $Time=Get-Date -Format "yyyy-MM-dd HH:mm:ss"

    "$Time  $Text" |
    Out-File $Global:LOGFILE -Append -Encoding utf8
}
