function Set-ConnectionStringsJson([string]$jsonPath, [string]$output, [string]$server, [string]$database,[string]$user,[string]$password)
{
	Write-Host $jsonPath
	Write-Host $output
	Write-Host $server
	Write-Host $database
	Write-Host $user
	Write-Host $password
	$json = Get-Content $jsonPath | ConvertFrom-Json
	$json.ConnectionStrings.EPiServerDB = "Data Source=$server;Initial Catalog=$database;User Id=$user;Password=$password;MultipleActiveResultSets=True" 
	$json | ConvertTo-Json -Depth 7 | set-content $output
}