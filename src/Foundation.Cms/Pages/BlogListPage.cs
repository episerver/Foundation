using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using Foundation.Cms.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(GroupName = CmsTabs.Blog,
        GUID = "EAADAFF2-3E89-4117-ADEB-F8D43565D2F4",
        DisplayName = "Blog Item List",
        Description = "Blog Item List for dates such as year and month")]
    [AvailableContentTypes(Availability.Specific, Include = new[] { typeof(BlogListPage), typeof(BlogItemPage) })]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-20.png")]
    public class BlogListPage : FoundationPageData
    {
        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual BlogListBlock BlogList { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Author { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual ContentArea LeftContentArea { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            BlogList.PageTypeFilter = typeof(BlogItemPage).GetPageType();
            BlogList.Recursive = true;
        }
    }
}