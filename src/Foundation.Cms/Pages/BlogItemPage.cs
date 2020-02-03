using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Blog Item Page",
        GUID = "EAACADF2-3E89-4117-ADEB-F8D43565D2F4",
        Description = "Blog Item Page created underneath the start page and moved to the right area",
        GroupName = CmsGroupNames.Blog)]
    [AvailableContentTypes(Availability.Specific, Include = new[] { typeof(BlogListPage), typeof(BlogItemPage) })]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-18.png")]
    public class BlogItemPage : FoundationPageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Author { get; set; }

        /// <summary>
        /// The comment section of the page. Local comment block will display comments only for this page
        /// </summary>
        [Display(Name = "Comment block",
            Description = "The comment section of the page. Local comment block will display comments only for this page",
            GroupName = SystemTabNames.Content,
            Order = 210)]
        public virtual BlogCommentBlock Comments { get; set; }
    }
}