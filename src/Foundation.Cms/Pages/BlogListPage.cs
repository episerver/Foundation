using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using Foundation.Cms.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Blog List Page", 
        GUID = "EAADAFF2-3E89-4117-ADEB-F8D43565D2F4",
        Description = "Blog List Page for dates such as year and month",
        GroupName = CmsTabNames.Blog)]
    [AvailableContentTypes(Availability.Specific, Include = new[] { typeof(BlogListPage), typeof(BlogItemPage) })]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-20.png")]
    public class BlogListPage : FoundationPageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Heading { get; set; }

        [Display(Name = "Blog list", GroupName = CmsTabNames.BlogList, Order = 11)]
        public virtual BlogListBlock BlogList { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            BlogList.PageTypeFilter = typeof(BlogItemPage).GetPageType();
            BlogList.Recursive = true;
        }
    }
}