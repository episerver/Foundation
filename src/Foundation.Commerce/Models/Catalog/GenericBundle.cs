using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(
        GUID = "F403ABFF-6C95-4B5B-AB7D-C15AE6055537",
        DisplayName = "Fashion Bundle",
        MetaClassName = "FashionBundle",
        Description = "Displays a bundle, which is collection of individual fashion variants.")]
    [ImageUrl("~/content/icons/pages/cms-icon-page-21.png")]
    public class GenericBundle : BundleContent, IProductRecommendations, IFoundationContent
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 5)]
        public virtual XhtmlString Description { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long description", Order = 10)]
        public virtual XhtmlString LongDescription { get; set; }

        [Display(Name = "On sale", Order = 15)]
        public virtual bool OnSale { get; set; }

        [Display(Name = "New arrival", Order = 20)]
        public virtual bool NewArrival { get; set; }

        [CultureSpecific]
        [Display(Name = "Content area", Order = 25)]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Associations title",
            Order = 30)]
        public virtual string AssociationsTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Show recommendations", Order = 35)]
        public virtual bool ShowRecommendations { get; set; }

        #region Implement IFoundationContent

        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = CmsTabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = CmsTabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ShowRecommendations = true;
            AssociationsTitle = "You May Also Like";
        }
    }
}
