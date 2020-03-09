using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(DisplayName = "Generic Package", GUID = "7b18ab7a-6344-4879-928e-e1b129d7379c", Description = "")]
    public class GenericPackage : PackageContent, IProductRecommendations, IFoundationContent
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 5)]
        public virtual XhtmlString Description { get; set; }

        [Display(Name = "On sale", Order = 10)]
        public virtual bool OnSale { get; set; }

        [Display(Name = "New arrival", Order = 15)]
        public virtual bool NewArrival { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long description", Order = 20)]
        public virtual XhtmlString LongDescription { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Content area",
            Description = "This will display the content area.",
            Order = 25)]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Associations title",
            Description = "This is title of the Associations tab.",
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

            AssociationsTitle = "You May Also Like";
        }
    }
}