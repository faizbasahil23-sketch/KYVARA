function Get-KyvaraConfig
{
    Get-Content `
        "C:\KYVARA\Builder\Config\builder.json" |
        ConvertFrom-Json
}
