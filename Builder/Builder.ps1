param(
    [string]$Command="help",
    [string]$Argument=""
)

$Root="C:\KYVARA"

#----------------------------------------------------------
# Load Services
#----------------------------------------------------------

. "$Root\Builder\Core\Services\Logger.ps1"
. "$Root\Builder\Core\Services\Config.ps1"
. "$Root\Builder\Core\Services\Registry.ps1"
. "$Root\Builder\Core\Services\Version.ps1"

#----------------------------------------------------------
# Load Core
#----------------------------------------------------------

. "$Root\Builder\Core\Discovery\Discovery.ps1"
. "$Root\Builder\Core\Loader\Loader.ps1"
. "$Root\Builder\Core\Resolver\Resolver.ps1"
. "$Root\Builder\Core\Installer\Installer.ps1"
. "$Root\Builder\Core\Commands\CommandEngine.ps1"

Write-KyvaraLog "KYVARA Builder Started"

switch($Command)
{

    "install"
    {
        Install-KyvaraModule $Argument
    }

    "verify"
    {
        Write-Host ""
        Write-Host "=================================" -ForegroundColor Cyan
        Write-Host "VERIFY MODULE : $Argument"
        Write-Host "=================================" -ForegroundColor Cyan

        Resolve-KyvaraDependency $Argument

        Write-Host ""
        Write-Host "Verification Finished." -ForegroundColor Green
    }

    "repair"
    {
        Write-Host ""
        Write-Host "Repair Module : $Argument"
    }

    "build"
    {
        Write-Host ""
        Write-Host "Build Module : $Argument"
    }

    "version"
    {
        Write-Host ""
        Write-Host "KYVARA Builder Version : $(Get-KyvaraVersion)"
    }

    default
    {
        Write-Host ""
        Write-Host "========================================"
        Write-Host "KYVARA Builder"
        Write-Host "========================================"
        Write-Host ""
        Write-Host "Commands:"
        Write-Host " install <module>"
        Write-Host " verify  <module>"
        Write-Host " repair  <module>"
        Write-Host " build   <module>"
        Write-Host " version"
        Write-Host ""
    }

}
