$ErrorActionPreference="Stop"

#==================================================
# KYVARA CLI v6 Enterprise
#==================================================

if([string]::IsNullOrWhiteSpace($PSScriptRoot))
{
    $ScriptRoot=(Get-Location).Path
}
else
{
    $ScriptRoot=$PSScriptRoot
}

. "$ScriptRoot\core\Core.ps1"

Show-Banner "KYVARA CLI v6"

function Invoke-Tool($Tool)
{
    $Script=Join-Path $ScriptRoot "$Tool.ps1"

    if(Test-Path $Script)
    {
        Write-Host ""
        Write-Host "Executing $Tool..." -ForegroundColor Cyan
        Write-Host ""

        & $Script
    }
    else
    {
        Write-Host ""
        Write-Host "$Tool.ps1 not found." -ForegroundColor Red
    }
}

function Show-Version
{
    Write-Host ""
    Write-Host "===================================" -ForegroundColor Cyan
    Write-Host "KYVARA CLI v6 Enterprise"
    Write-Host "===================================" -ForegroundColor Cyan
    Write-Host ""

    Write-Host "Root      :" $Global:KYVARA_ROOT
    Write-Host "Solution  :" $Global:SOLUTION
    Write-Host "Database  :" $Global:DATABASE
    Write-Host ""

    dotnet --version
}

function Show-Menu
{
Write-Host ""
Write-Host "doctor"
Write-Host "repair"
Write-Host "build"
Write-Host "run"
Write-Host "watch"
Write-Host "deploy"
Write-Host "package"
Write-Host "stats"
Write-Host "analyze"
Write-Host "heal"
Write-Host "backup"
Write-Host "restore"
Write-Host "publish"
Write-Host "info"
Write-Host "version"
Write-Host "help"
Write-Host ""
}

if($args.Count -eq 0)
{
    Show-Menu
    exit
}

switch($args[0].ToLower())
{

"doctor"{

Invoke-Tool "doctor"

}

"repair"{

Invoke-Tool "repair"

}

"build"{

Invoke-Tool "build"

}

"run"{

Invoke-Tool "run"

}

"watch"{

Invoke-Tool "watch"

}

"deploy"{

Invoke-Tool "deploy"

}

"publish"{

Invoke-Tool "publish"

}

"backup"{

Invoke-Tool "backup"

}

"restore"{

Invoke-Tool "restore"

}

"package"{

Invoke-Tool "package"

}

"stats"{

Invoke-Tool "stats"

}

"analyze"{

Invoke-Tool "analyze"

}

"heal"{

Invoke-Tool "heal"

}

"info"{

Show-Version

}

"version"{

Show-Version

}

"help"{

Show-Menu

}

#==========================
# ALIAS
#==========================

"d"{

Invoke-Tool "doctor"

}

"r"{

Invoke-Tool "repair"

}

"b"{

Invoke-Tool "build"

}

"rr"{

Invoke-Tool "run"

}

"w"{

Invoke-Tool "watch"

}

"p"{

Invoke-Tool "publish"

}

"a"{

Invoke-Tool "analyze"

}

"s"{

Invoke-Tool "stats"

}

"h"{

Invoke-Tool "heal"

}

"pkg"{

Invoke-Tool "package"

}

"dep"{

Invoke-Tool "deploy"

}

"bk"{

Invoke-Tool "backup"

}

"res"{

Invoke-Tool "restore"

}

#==========================
# NEW MODULE
#==========================

"new"{

if($args.Count -lt 3)
{
Write-Host ""
Write-Host "Usage:"
Write-Host ""
Write-Host "kyvara new module User"
Write-Host "kyvara new feature Login"
Write-Host "kyvara new entity Product"
Write-Host "kyvara new aggregate Order"
Write-Host ""
break
}

$type=$args[1].ToLower()
$name=$args[2]

switch($type)
{

"module"{

Invoke-Tool "new-module"

}

"feature"{

Invoke-Tool "new-feature"

}

"entity"{

Invoke-Tool "new-entity"

}

"aggregate"{

Invoke-Tool "new-aggregate"

}

default{

Write-Host ""
Write-Host "Unknown template."

}

}

}

default{

Write-Host ""
Write-Host "Unknown command."
Write-Host ""
Show-Menu

}

}

