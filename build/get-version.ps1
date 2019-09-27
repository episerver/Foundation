[xml] $versionFile = Get-Content "$PSScriptRoot\version.props"
return $versionFile.SelectSingleNode("Project/PropertyGroup/VersionPrefix").InnerText
