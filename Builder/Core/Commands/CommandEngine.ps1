function Invoke-KyvaraCommand
{
    param(

        [string]$Command,

        [string]$Argument

    )

    switch($Command)
    {

        "install"
        {
            Install-KyvaraModule $Argument
        }

        "verify"
        {
            Write-Host "Verify : $Argument"
        }

        "repair"
        {
            Write-Host "Repair : $Argument"
        }

        "build"
        {
            Write-Host "Build : $Argument"
        }

        default
        {
            Write-Host "Unknown command."
        }

    }

}
