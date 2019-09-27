using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Blog Comment Block", GUID = "656ff547-1c31-4fc1-99b9-93573d24de07", Description = "")]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-25.png")]
    public class BlogCommentBlock : BlockData
    {
        [DefaultValue(5)]
        [Display(
                    Name = "RecordPerPage",
                    Description = "Count of comments per page",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
        public virtual int RecordPerPage { get; set; }
    }
}