function Get-KyvaraRegistry
{
    Get-Content `
        "C:\KYVARA\Builder\Registry\modules.json" |
        ConvertFrom-Json
}
