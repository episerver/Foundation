using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blog.BlogCommentBlock
{
    [ContentType(DisplayName = "Blog Comment Block",
        GUID = "656ff547-1c31-4fc1-99b9-93573d24de07",
        Description = "Configures the frontend view properties of a blog comment block",
        GroupName = GroupNames.Blog, Order = 10)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-25.png")]
    public class BlogCommentBlock : BlockData
    {
        [Range(0, 1000)]
        [Display(Name = "Comments per page", Description = "Number of comments per page", GroupName = SystemTabNames.Content)]
        public virtual int CommentsPerPage { get; set; }

        [Display(Name = "Padding top", Order = 20)]
        public virtual int PaddingTop { get; set; }

        [Display(Name = "Padding right", Order = 21)]
        public virtual int PaddingRight { get; set; }

        [Display(Name = "Padding bottom", Order = 22)]
        public virtual int PaddingBottom { get; set; }

        [Display(Name = "Padding left", Order = 23)]
        public virtual int PaddingLeft { get; set; }

        public string PaddingStyles
        {
            get
            {
                var paddingStyles = "";

                paddingStyles += PaddingTop > 0 ? "padding-top: " + PaddingTop + "px;" : "";
                paddingStyles += PaddingRight > 0 ? "padding-right: " + PaddingRight + "px;" : "";
                paddingStyles += PaddingBottom > 0 ? "padding-bottom: " + PaddingBottom + "px;" : "";
                paddingStyles += PaddingLeft > 0 ? "padding-left: " + PaddingLeft + "px;" : "";

                return paddingStyles;
            }
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            CommentsPerPage = 5;
            PaddingTop = 0;
            PaddingRight = 0;
            PaddingBottom = 0;
            PaddingLeft = 0;
        }
    }
}