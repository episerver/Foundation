# NOTE: This script must currently be executed from the solution dir (due to specs)
Param([string]$server, [string]$additionalSQL)
$ErrorActionPreference = "Stop"

# Set location to the Solution directory
(Get-Item $PSScriptRoot).Parent.FullName | Push-Location

Import-Module .\build\exechelper.ps1

# Install .NET tooling

exec .\build\dotnet-cli-install.ps1

$ErrorActionPreference = "continue"
# Get cli tool
dotnet "tool install EPiServer.Net.Cli --global --add-source https://pkgs.dev.azure.com/EpiserverEngineering/netCore/_packaging/beta-program/nuget/v3/index.json --version 1.0.0-inte-020009"
$ErrorActionPreference = "Stop"
dotnet-episerver create-cms-database ".\src\Foundation\Foundation.csproj" -S "$server" $additionalSQL
dotnet-episerver create-commerce-database ".\src\Foundation\Foundation.csproj" -S "$server" $additionalSQL --reuse-cms-user

Pop-Location