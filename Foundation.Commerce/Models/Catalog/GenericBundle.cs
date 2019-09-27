using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(
        GUID = "F403ABFF-6C95-4B5B-AB7D-C15AE6055537",
        DisplayName = "Fashion Bundle",
        MetaClassName = "FashionBundle",
        Description = "Displays a bundle, which is collection of individual fashion variants.")]
    [ImageUrl("~/content/icons/pages/cms-icon-page-21.png")]
    public class GenericBundle : BundleContent, IProductRecommendations
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 1)]
        public virtual XhtmlString Description { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long Description", Order = 3)]
        public virtual XhtmlString LongDescription { get; set; }

        [Display(Name = "On Sale", Order = 2, Description = "Is on sale?")]
        public virtual bool OnSale { get; set; }

        [Display(Name = "New Arrival", Order = 2, Description = "Is on a new arrival?")]
        public virtual bool NewArrival { get; set; }

        [CultureSpecific]
        [Display(Name = "Content Area", Order = 3, Description = "This will display the content area.")]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Show Recommendations", Order = 50, Description = "This will determine whether or not to show recommendations.")]
        public virtual bool ShowRecommendations { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            ShowRecommendations = true;
        }
    }
}
