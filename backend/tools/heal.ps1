. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA AI SELF HEALING ENGINE"

Write-Log "Self Healing Started"

$Start=Get-Date

$Success=@()
$Failed=@()

function Step($Title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Cyan
    Write-Host $Title -ForegroundColor Yellow
    Write-Host "===================================================" -ForegroundColor Cyan
}

function RunScript($Name)
{
    $Script=Join-Path $PSScriptRoot "$Name.ps1"

    if(Test-Path $Script)
    {
        try
        {
            Write-Host ""
            Write-Host "Running $Name..." -ForegroundColor Cyan

            & $Script

            $script:Success+=$Name

            Write-Host ""
            Write-Host "$Name SUCCESS" -ForegroundColor Green
        }
        catch
        {
            $script:Failed+=$Name

            Write-Host ""
            Write-Host "$Name FAILED" -ForegroundColor Red
            Write-Host $_.Exception.Message -ForegroundColor Yellow
        }
    }
    else
    {
        $script:Failed+=$Name

        Write-Host "$Name.ps1 not found." -ForegroundColor Yellow
    }
}

Step "PROJECT DISCOVERY"

Write-Host "Root     :" $Global:KYVARA_ROOT
Write-Host "Solution :" $Global:SOLUTION

Step "DOCTOR"

RunScript "doctor"

Step "PACKAGE ANALYSIS"

RunScript "package"

Step "PROJECT ANALYSIS"

RunScript "analyze"

Step "REPAIR"

RunScript "repair"

Step "BUILD"

RunScript "build"

Step "DATABASE"

try
{
    dotnet ef database update `
    --project "$Global:KYVARA_ROOT\Kyvara.Infrastructure" `
    --startup-project "$Global:KYVARA_ROOT\Kyvara.Api"

    Write-Host ""
    Write-Host "Database synchronized." -ForegroundColor Green
}
catch
{
    Write-Host ""
    Write-Host "Database update skipped." -ForegroundColor Yellow
}

Step "BACKUP"

RunScript "backup"

Step "FINAL BUILD"

try
{
    dotnet build "$Global:SOLUTION" -c Release

    Write-Host ""
    Write-Host "Final Build Success." -ForegroundColor Green
}
catch
{
    Write-Host ""
    Write-Host "Final Build Failed." -ForegroundColor Red
}

$End=Get-Date

$Elapsed=$End-$Start

Write-Host ""
Write-Host "===================================================" -ForegroundColor Green
Write-Host "KYVARA AI SELF HEALING REPORT" -ForegroundColor Green
Write-Host "===================================================" -ForegroundColor Green

Write-Host ""
Write-Host "Started  :" $Start
Write-Host "Finished :" $End
Write-Host "Duration :" $Elapsed

Write-Host ""
Write-Host "Succeeded :" $Success.Count
Write-Host "Failed    :" $Failed.Count

Write-Host ""

if($Success.Count -gt 0)
{
    Write-Host "SUCCESS"

    $Success | Sort-Object
}

Write-Host ""

if($Failed.Count -gt 0)
{
    Write-Host "FAILED"

    $Failed | Sort-Object
}

$ReportFolder="$Global:KYVARA_ROOT\reports"

if(!(Test-Path $ReportFolder))
{
    New-Item $ReportFolder -ItemType Directory | Out-Null
}

$Report="$ReportFolder\SelfHealingReport.txt"

@"

KYVARA SELF HEALING REPORT

Started : $Start

Finished : $End

Duration : $Elapsed

Succeeded : $($Success.Count)

Failed : $($Failed.Count)

SUCCESS

$($Success -join "`r`n")

FAILED

$($Failed -join "`r`n")

"@ | Out-File $Report -Encoding UTF8

Write-Host ""
Write-Host "Report :" $Report -ForegroundColor Green

Write-Host ""
Write-Host "AI SELF HEALING COMPLETED." -ForegroundColor Green

Write-Log "Self Healing Finished"

