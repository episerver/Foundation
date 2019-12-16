﻿using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Navigation Block",
        GUID = "7C53F707-C932-4FDD-A654-37FF2A1258EB",
        GroupName = CmsGroupNames.Content)]
    [SiteImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-30.png")]
    public class NavigationBlock : FoundationBlockData
    {
        [Display(Name = "Heading", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(Name = "Root page", Order = 20, GroupName = SystemTabNames.Content)]
        public virtual PageReference RootPage { get; set; }
    }
}