function Invoke-Dotnet($Arguments)
{
    Write-Host ""
    Write-Host "dotnet $Arguments" -ForegroundColor Cyan
    Write-Host ""

    Invoke-Expression "dotnet $Arguments"

    if($LASTEXITCODE -ne 0)
    {
        throw "Dotnet command failed."
    }
}
