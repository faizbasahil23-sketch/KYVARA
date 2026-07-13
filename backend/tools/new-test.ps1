. "$PSScriptRoot\core\Core.ps1"

param(
[string]$Name="Sample"
)

Show-Banner "UNIT TEST"

$Folder="$Global:KYVARA_ROOT\Tests"

New-Item $Folder -ItemType Directory -Force | Out-Null

@"
using Xunit;

public class ${Name}Tests
{

}
"@ | Set-Content "$Folder\$Name.Tests.cs"

Write-Host "Unit Test Created"

