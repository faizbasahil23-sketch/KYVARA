Write-Host "Importing Project..."

Get-ChildItem .. -Recurse -Include *.cs,*.csproj,*.json | Out-Null

Write-Host "Import Completed."
