using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Blog Comment Block", GUID = "656ff547-1c31-4fc1-99b9-93573d24de07", GroupName = CmsTabNames.Blog)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-25.png")]
    public class BlogCommentBlock : BlockData
    {
        [Range(0, 1000)]
        [Display(Name = "Number of comments", Description = "Number of comments per page", GroupName = SystemTabNames.Content)]
        public virtual int NumberOfComments { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            NumberOfComments = 5;
        }
    }
}