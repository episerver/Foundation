$ErrorActionPreference = "Stop"

# set execute directory to root
$RootDir = (Get-Item $PSScriptRoot).Parent.FullName
Push-Location $RootDir

$outputRoot = "$RootDir\Output"
$githubRoot = "$outputRoot\Github"
$contentRoot = "$githubRoot\Content"

Remove-Item $githubRoot -Recurse -ErrorAction Ignore
New-Item -ItemType directory -Path "$githubRoot"
New-Item -ItemType directory -Path "$contentRoot"
New-Item -ItemType directory -Path "$githubRoot\Foundation.Cms.Personalization"
New-Item -ItemType directory -Path "$githubRoot\Foundation.Commerce.Personalization"

$ExcludeFolders = "obj","bin"
Get-ChildItem -Path "$RootDir\Foundation.Campaign" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Campaign" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Cms" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Cms" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Cms.Personalization" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Cms.Personalization" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Commerce" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Commerce" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Commerce.Personalization" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Commerce.Personalization" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Find.Cms" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Find.Cms" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Find.Commerce" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Find.Commerce" -force -recurse
Get-ChildItem -Path "$RootDir\Foundation.Social" -Exclude $ExcludeFolders | Copy-Item -Destination "$githubRoot\Foundation.Social" -force -recurse

Copy-Item -Path "$outputRoot\Foundation.Cms" -Recurse -Force -Destination "$contentRoot\Foundation.Cms"
Copy-Item -Path "$outputRoot\Foundation.Cms.Personalization" -Recurse -Force -Destination "$contentRoot\Foundation.Cms.Personalization"
Copy-Item -Path "$outputRoot\Foundation.Commerce" -Recurse -Force -Destination "$contentRoot\Foundation.Commerce"
Copy-Item -Path "$outputRoot\Foundation.Commerce.Personalization" -Recurse -Force -Destination "$contentRoot\Foundation.Commerce.Personalization"
Copy-Item -Path "$outputRoot\Foundation.Find.Cms" -Recurse -Force -Destination "$contentRoot\Foundation.Find.Cms"
Copy-Item -Path "$outputRoot\Foundation.Find.Commerce" -Recurse -Force -Destination "$contentRoot\Foundation.Find.Commerce"
Copy-Item -Path "$outputRoot\Foundation.Social" -Recurse -Force -Destination "$contentRoot\Foundation.Social"

Compress-Archive -Path $githubRoot -DestinationPath "$RootDir\artifacts\github"
