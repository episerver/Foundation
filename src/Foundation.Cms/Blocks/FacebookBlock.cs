using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Facebook Feed Block", GUID = "fe935bfb-44b0-4ce2-a448-1d366ff3bbc0", GroupName = CmsTabNames.SocialMedia)]
    [ImageUrl("~/assets/icons/cms/blocks/rss.png")]
    public class FacebookBlock : FoundationBlockData
    {
        [Display(Name = "Account name", GroupName = SystemTabNames.Content)]
        public virtual string AccountName { get; set; }
    }
}