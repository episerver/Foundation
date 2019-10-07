$ErrorActionPreference = "Stop"

# set execute directory to root
$RootDir = (Get-Item $PSScriptRoot).Parent.FullName
Push-Location $RootDir

$sourceRoot = ".\src\Foundation"
$destinationRoot = ".\artifacts\Output"

Remove-Item $destinationRoot -Recurse -ErrorAction Ignore
New-Item -ItemType directory -Path "$destinationRoot"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms.Personalization"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Commerce"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Commerce.Personalization"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Find.Cms"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Find.Commerce"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Social"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms\lang"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms\Features\Blocks"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms\Features\Header"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms\Features\Pages"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\DisplayTemplates"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Commerce\Assets\js"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Commerce\Assets\scss\templates"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Find.Cms\Features"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Social\Features\Blocks\Views"
New-Item -ItemType directory -Path "$destinationRoot\Foundation.Cms.Personalization\Features\Blocks\Views"

Copy-Item -Path $sourceRoot -Recurse -Destination $destinationRoot -Force

$classes = Get-ChildItem "$destinationRoot\Foundation" *.cs -rec -Force
foreach ($file in $classes)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "namespace Foundation", "namespace {projectname}" } |
    Set-Content $file.PSPath
}

#Foundation.Cms
Copy-Item "$destinationRoot\Foundation\Assets" -Force -Recurse -Destination "$destinationRoot\Foundation.Cms\Assets"
    
Get-ChildItem -Path "$destinationRoot\Foundation.Cms\Assets\js\features" -Recurse -Exclude "Blog.js", "Dropdown.js", "foundation.cms.js", "Header.js", "mobile-navigation.js", "notify.js", "SearchBox.js", "Selection.js"  | ForEach-Object ($_) {
    Remove-Item $_.fullname -Force -Recurse
}

Get-ChildItem -Path "$destinationRoot\Foundation.Cms\Assets\scss\templates\pages" -Recurse -Exclude "_blog-item.scss", "_blog-list.scss", "_home.scss", "_standard-page.scss"  | ForEach-Object ($_) {
    Remove-Item $_.fullname -Force -Recurse
}

Get-ChildItem -Path "$destinationRoot\Foundation.Cms\Assets\scss\templates\widgets" -Recurse -Exclude "_toolbar.scss", "_breadcrumb.scss", "_logged-in-user.scss", "_login-users.scss"  | ForEach-Object ($_) {
    Remove-Item $_.fullname -Force -Recurse
}

Copy-Item "Build\Content\Foundation.Cms\foundation.init.js" -Destination "$destinationRoot\Foundation.Cms\Assets\js\features\foundation.init.js" -Force -Recurse -Container | Out-Null
Copy-Item "$destinationRoot\Foundation\ClientResources" -Destination "$destinationRoot\Foundation.Cms\ClientResources" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Helpers" -Destination "$destinationRoot\Foundation.Cms\Helpers" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\lang\Foundation.Core_EN.xml" -Destination "$destinationRoot\Foundation.Cms\lang\Foundation.Core_EN.xml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\modules\_protected\Foundation.Cms" -Destination "$destinationRoot\Foundation.Cms\modules\_protected\Foundation.Cms" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\gulpfile.js" -Destination "$destinationRoot\Foundation.Cms\gulpfile.js" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\module.config" -Destination "$destinationRoot\Foundation.Cms\module.config" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blocks\DefaultBlockController.cs" -Destination "$destinationRoot\Foundation.Cms\Features\Blocks\DefaultBlockController.cs" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blocks\PageListBlockController.cs" -Destination "$destinationRoot\Foundation.Cms\Features\Blocks\PageListBlockController.cs" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blocks\RssReaderBlockController.cs" -Destination "$destinationRoot\Foundation.Cms\Features\Blocks\RssReaderBlockController.cs" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blocks\Views" -Destination "$destinationRoot\Foundation.Cms\Features\Blocks\Views" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blog" -Destination "$destinationRoot\Foundation.Cms\Features\Blog" -Exclude BlogCommentBlock -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Events" -Destination "$destinationRoot\Foundation.Cms\Features\Events" -Force -Recurse
Copy-Item "Build\Content\Foundation.Cms\HeaderController.cs" -Destination "$destinationRoot\Foundation.Cms\Features\Header\HeaderController.cs" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Home" -Destination "$destinationRoot\Foundation.Cms\Features\Home" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Media" -Destination "$destinationRoot\Foundation.Cms\Features\Media" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Pages\LandingPage" -Destination "$destinationRoot\Foundation.Cms\Features\Pages\LandingPage" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Pages\StandardPage" -Destination "$destinationRoot\Foundation.Cms\Features\Pages\StandardPage" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Pages\ThreeColumnLandingPage" -Destination "$destinationRoot\Foundation.Cms\Features\Pages\ThreeColumnLandingPage" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Pages\TwoColumnLandingPage" -Destination "$destinationRoot\Foundation.Cms\Features\Pages\TwoColumnLandingPage" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Preview" -Destination "$destinationRoot\Foundation.Cms\Features\Preview" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\DisplayTemplates\HeroBlockCallout.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\DisplayTemplates\HeroBlockCallout.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\ElementBlocks" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\ElementBlocks" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\Header" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\Header" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\_BreadCrumb.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\_BreadCrumb.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\_Footer.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\_Footer.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\_Layout.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\_Layout.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\_MasterLayout.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\_MasterLayout.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\_Page.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\_Page.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Shared\Foundation\_Pager.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\Shared\Foundation\_Pager.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\_viewstart.cshtml" -Destination "$destinationRoot\Foundation.Cms\Features\_viewstart.cshtml" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Web.config" -Destination "$destinationRoot\Foundation.Cms\Features\Web.config"

Remove-Item "$destinationRoot\Foundation.Cms\Assets\js\main.min.js" -Force -ErrorAction Ignore
Remove-Item "$destinationRoot\Foundation.Cms\Assets\js\main.min.js.map" -Force -ErrorAction Ignore
Remove-Item "$destinationRoot\Foundation.Cms\Assets\scss\main.min.css" -Force -ErrorAction Ignore
Remove-Item "$destinationRoot\Foundation.Cms\Assets\scss\main.min.css.map" -Force -ErrorAction Ignore
Remove-Item "$destinationRoot\Foundation.Cms\Features\Blocks\Views\ContentRecommendationsBlock.cshtml" -Force
Remove-Item "$destinationRoot\Foundation.Cms\Features\Blocks\Views\ElevatedRoleBlock.cshtml" -Force
Remove-Item "$destinationRoot\Foundation.Cms\Features\Blocks\Views\MembershipAffiliationBlock.cshtml" -Force
Remove-Item "$destinationRoot\Foundation.Cms\Features\Blocks\Views\RecentPageCategoryRecommendation.cshtml" -Force

#Foundation.Commerce
Copy-Item "$destinationRoot\Foundation\Assets\js\features" -Force -Recurse -Destination "$destinationRoot\Foundation.Commerce\Assets\js\features"
"Blog.js", "Dropdown.js", "foundation.cms.js", "Header.js", "mobile-navigation.js", "notify.js", "SearchBox.js", "Selection.js", "foundation.cms.personalization.js" | ForEach-Object {
    Remove-Item $destinationRoot\Foundation\Assets\js\features\$_ -Force -Recurse
}    

Copy-Item "$destinationRoot\Foundation\Assets\scss\templates\pages" -Force -Recurse -Destination "$destinationRoot\Foundation.Commerce\Assets\templates\pages"
"_blog-item.scss", "_blog-list.scss", "_home.scss", "_standard-page.scss", "_location.scss"  | ForEach-Object ($_) {
    Remove-Item $destinationRoot\Foundation\Assets\scss\templates\pages\$_ -Force -Recurse
}

Copy-Item "$destinationRoot\Foundation\Assets\scss\templates\widgets" -Force -Recurse -Destination "$destinationRoot\Foundation.Commerce\Assets\templates\widgets"
"_toolbar.scss", "_breadcrumb.scss", "_logged-in-user.scss", "_login-users.scss"  | ForEach-Object ($_) {
    Remove-Item $destinationRoot\Foundation\Assets\scss\templates\widgets\$_ -Force -Recurse
}

Copy-Item "$destinationRoot\Foundation\features" -Force -Recurse -Destination "$destinationRoot\Foundation.Commerce\Features"
"blog", "events", "home", "locations", "media", "pages", "preview", "recommendations","search", "setup"  | ForEach-Object ($_) {
    Remove-Item $destinationRoot\Foundation.Commerce\Features\$_ -Force -Recurse
}

Copy-Item "$destinationRoot\Foundation\modules\_protected\Foundation.Commerce" -Destination "$destinationRoot\Foundation.Commerce\modules\_protected\Foundation.Commerce" -Force -Recurse

#Foundation.Find.Cms
Copy-Item "Build\Content\Foundation.Find.Cms\Search" -Force -Recurse -Destination "$destinationRoot\Foundation.Find.Cms\Features\Search"
Copy-Item  "$destinationRoot\Foundation\Features\Locations" -Force -Recurse -Destination "$destinationRoot\Foundation.Find.Cms\Features\Locations"

#Foundation.Find.Commerce
Copy-Item "$destinationRoot\Foundation\Features\Search" -Force -Recurse -Destination "$destinationRoot\Foundation.Find.Commerce\Features\Search"

#Foundation.Social
Copy-Item "$destinationRoot\Foundation\modules\_protected\Foundation.Social" -Destination "$destinationRoot\Foundation.Social\modules\_protected\Foundation.Social" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blog\BlogCommentBlock" -Destination "$destinationRoot\Foundation.Social\Features\Blog\BlogCommentBlock" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blocks\Views\MembershipAffiliationBlock.cshtml" -Destination "$destinationRoot\Foundation.Social\Features\Blocks\Views\MembershipAffiliationBlock.cshtml" -Force -Recurse

#Foundation.Cms.Personalization
Copy-Item "$destinationRoot\Foundation\Features\Blocks\ContentRecommendationsBlockController.cs" -Destination "$destinationRoot\Foundation.Cms.Personalization\Features\Blocks\ContentRecommendationsBlockController.cs" -Force -Recurse
Copy-Item "$destinationRoot\Foundation\Features\Blocks\Views\ContentRecommendationsBlock.cshtml" -Destination "$destinationRoot\Foundation.Cms.Personalization\Features\Blocks\Views\ContentRecommendationsBlock.cshtml" -Force -Recurse

#Foundation.Commerce.Personalization
Copy-Item "$destinationRoot\Foundation\Features\Recommendations" -Destination "$destinationRoot\Foundation.Commerce.Personalization\Features\Recommendations" -Force -Recurse