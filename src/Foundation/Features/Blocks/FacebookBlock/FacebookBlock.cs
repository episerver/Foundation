using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.FacebookBlock
{
    [ContentType(DisplayName = "Facebook Feed Block",
        GUID = "fe935bfb-44b0-4ce2-a448-1d366ff3bbc0",
        Description = "Display content from a Facebook feed",
        GroupName = GroupNames.SocialMedia)]
    [ImageUrl("/icons/cms/blocks/rss.png")]
    public class FacebookBlock : FoundationBlockData
    {
        [Display(Name = "Account name", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string AccountName { get; set; }

        [Range(180, 500, ErrorMessage = "Min width is 180 & Max width is 500")]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual int Width { get; set; }

        [Range(70, 5000, ErrorMessage = "Min width is 70 & Max width is 5000")]
        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual int Height { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Width = 340;
            Height = 500;
        }
    }
}