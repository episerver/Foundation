using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Blog Comment Block", GUID = "656ff547-1c31-4fc1-99b9-93573d24de07", GroupName = CmsGroupNames.Blog)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-25.png")]
    public class BlogCommentBlock : FoundationBlockData
    {
        [Range(0, 1000)]
        [Display(Name = "Comments per page", Description = "Number of comments per page", GroupName = SystemTabNames.Content)]
        public virtual int CommentsPerPage { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            CommentsPerPage = 5;
        }
    }
}