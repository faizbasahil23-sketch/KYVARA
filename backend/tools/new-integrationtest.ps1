. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="Sample"
)

Show-Banner "INTEGRATION TEST"

$Folder="$Global:KYVARA_ROOT\Tests\Integration"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
using Xunit;

public class ${Name}IntegrationTests
{

}
"@ | Set-Content "$Folder\$Name.IntegrationTests.cs"

Write-Host "Integration Test Created"

