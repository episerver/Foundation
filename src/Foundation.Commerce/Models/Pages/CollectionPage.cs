using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
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
    public class CollectionPage : FoundationPageData
    {
        [AllowedTypes(typeof(BreadcrumbBlock))]
        [Display(Name = "Breadcrumb block", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual ContentArea Navigation { get; set; }

        [AllowedTypes(typeof(MediaBlock))]
        [Display(Name = "Media", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual ContentArea Media { get; set; }

        [AllowedTypes(new[] { typeof(CategoryBlock), typeof(ProductSearchBlock) })]
        [Display(Name = "Products", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentArea Products { get; set; }
    }
}