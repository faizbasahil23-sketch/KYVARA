function Get-KyvaraModules
{
    $registry="C:\KYVARA\Builder\Registry\modules.json"

    if(!(Test-Path $registry))
    {
        return @()
    }

    return Get-Content $registry | ConvertFrom-Json
}
