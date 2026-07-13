function Get-KyvaraRegistryPath
{
    return "C:\KYVARA\Builder\Registry\modules.json"
}

function Get-KyvaraRegistry
{
    $file=Get-KyvaraRegistryPath

    if(!(Test-Path $file))
    {
        @() | ConvertTo-Json | Set-Content $file
    }

    $text=Get-Content $file -Raw

    if([string]::IsNullOrWhiteSpace($text))
    {
        return @()
    }

    return $text | ConvertFrom-Json
}

function Save-KyvaraRegistry
{
    param($Registry)

    $Registry |
    ConvertTo-Json -Depth 10 |
    Set-Content (Get-KyvaraRegistryPath)
}

function Register-KyvaraModule
{
    param(

        [string]$Name,

        [string]$Version,

        [string]$Description

    )

    $registry=@(Get-KyvaraRegistry)

    $exist=$registry |
    Where-Object Name -eq $Name

    if($exist)
    {
        Write-Host "$Name already registered."
        return
    }

    $registry+=@{

        Name=$Name

        Version=$Version

        Description=$Description

        Installed=$false

    }

    Save-KyvaraRegistry $registry

    Write-Host "$Name registered."
}

function Get-KyvaraModule
{
    param([string]$Name)

    Get-KyvaraRegistry |
    Where-Object Name -eq $Name
}
