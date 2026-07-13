. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA ENTERPRISE BUILD"

Write-Log "Build Started"

$sw=[System.Diagnostics.Stopwatch]::StartNew()

$Succeeded=$true

function Section($Title)
{
    Write-Host ""
    Write-Host "===================================================" -ForegroundColor Cyan
    Write-Host $Title -ForegroundColor Yellow
    Write-Host "===================================================" -ForegroundColor Cyan
}

function Run($Command)
{
    Write-Host ""
    Write-Host $Command -ForegroundColor Gray
    Write-Host ""

    Invoke-Expression $Command

    if($LASTEXITCODE -ne 0)
    {
        $script:Succeeded=$false
        throw "Command failed."
    }
}

Section "PROJECT"

Write-Host "Root      :" $Global:KYVARA_ROOT
Write-Host "Solution  :" $Global:SOLUTION
Write-Host "Database  :" $Global:DATABASE

Section "VERIFY"

if(!(Test-Path $Global:SOLUTION))
{
    throw "Solution not found."
}

Section "CLEAN"

Run "dotnet clean `"$Global:SOLUTION`""

Section "RESTORE"

Run "dotnet restore `"$Global:SOLUTION`""

Section "BUILD DEBUG"

Run "dotnet build `"$Global:SOLUTION`" -c Debug --no-restore"

Section "BUILD RELEASE"

Run "dotnet build `"$Global:SOLUTION`" -c Release --no-restore"

Section "PROJECTS"

Get-ChildItem $Global:KYVARA_ROOT -Recurse *.csproj |
ForEach-Object{

Write-Host $_.Name

}

Section "SDK"

dotnet --version

Section "SUMMARY"

$sw.Stop()

Write-Host ""

if($Succeeded)
{
    Write-Host "STATUS     : SUCCESS" -ForegroundColor Green
}
else
{
    Write-Host "STATUS     : FAILED" -ForegroundColor Red
}

Write-Host "TIME       :" $sw.Elapsed

Write-Host "PROJECTS   :" (
Get-ChildItem $Global:KYVARA_ROOT -Recurse *.csproj
).Count

Write-Host "SOLUTION   :" (
Split-Path $Global:SOLUTION -Leaf
)

Write-Host ""

Write-Log "Build Finished"

Write-Host ""
Write-Host "BUILD COMPLETE" -ForegroundColor Green
