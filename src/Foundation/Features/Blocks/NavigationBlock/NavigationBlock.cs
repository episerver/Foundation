using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.NavigationBlock
{
    [ContentType(DisplayName = "Navigation Block",
        GUID = "7C53F707-C932-4FDD-A654-37FF2A1258EB",
        Description = "Render normal left/right navigation structures",
        GroupName = GroupNames.Content)]
    [SiteImageUrl("/icons/cms/blocks/CMS-icon-block-30.png")]
    public class NavigationBlock : FoundationBlockData
    {
        [Display(Name = "Heading", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(Name = "Root page", Order = 20, GroupName = SystemTabNames.Content)]
        public virtual PageReference RootPage { get; set; }
    }
}