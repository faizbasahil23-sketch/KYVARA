. "$PSScriptRoot\core\Core.ps1"

Show-Banner "KYVARA DEVOPS ENGINE"

$DevOps="$Global:KYVARA_ROOT\DevOps"

New-Item $DevOps -ItemType Directory -Force | Out-Null

$Folders=@(
"Docker",
"Kubernetes",
"GitHub",
"Azure",
"Monitoring",
"Logging",
"Scripts",
"Terraform"
)

foreach($f in $Folders)
{
    New-Item "$DevOps\$f" -ItemType Directory -Force | Out-Null
}

Write-Host ""
Write-Host "DevOps Folder Created." -ForegroundColor Green
