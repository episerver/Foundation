using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Foundation.Cms.Blocks;
using Foundation.Cms.Pages;
using Foundation.Commerce.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Collection Page",
        GUID = "e5c11d0c-6932-4888-a610-1474e73b66d1",
        Description = "Collection page",
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-04.png")]
    public class CollectionPage : FoundationPageData
    {
        [AllowedTypes(typeof(BreadcrumbBlock))]
        [Display(Name = "Navigation", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual ContentArea Navigation { get; set; }

        [Display(Name = "Name", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string CollectionName { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Image", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentReference Image { get; set; }

        [UIHint(UIHint.Video)]
        [Display(Name = "Video", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual ContentReference Video { get; set; }

        [Display(Name = "Description", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual XhtmlString Description { get; set; }

        [AllowedTypes(new[] { typeof(CategoryBlock), typeof(ProductSearchBlock) })]
        [Display(Name = "Products", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual ContentArea Products { get; set; }
    }
}