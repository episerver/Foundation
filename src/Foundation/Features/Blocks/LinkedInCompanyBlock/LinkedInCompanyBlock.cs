using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.LinkedInCompanyBlock
{
    [ContentType(DisplayName = "Linkedin Feed Block",
        GUID = "419db9dd-44bc-4540-b446-fcb5f6d588fa",
        Description = "Display content from a Linkedin feed",
        GroupName = GroupNames.SocialMedia)]
    [ImageUrl("~/assets/icons/cms/blocks/rss.png")]
    public class LinkedInCompanyBlock : FoundationBlockData
    {
        [Display(Name = "Account name", GroupName = SystemTabNames.Content)]
        public virtual string AccountName { get; set; }
    }
}