using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Linkedin Company Block", GUID = "419db9dd-44bc-4540-b446-fcb5f6d588fa", GroupName = CmsTabNames.SocialMedia)]
    [ImageUrl("~/assets/icons/cms/blocks/rss.png")]
    public class LinkedInCompanyBlock : FoundationBlockData
    {
        [Display(Name = "Company name", GroupName = SystemTabNames.Content)]
        public virtual string CompanyName { get; set; }
    }
}