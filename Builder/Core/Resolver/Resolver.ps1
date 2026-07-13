function Resolve-KyvaraDependency
{
    param(
        [string]$Module
    )

    Write-Host "Resolving dependency : $Module"

    return $true
}
