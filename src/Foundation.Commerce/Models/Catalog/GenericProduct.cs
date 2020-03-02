using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms;
using Foundation.Commerce.Models.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(
        GUID = "e638670d-3f73-4867-8745-1125dcc066ca",
        MetaClassName = "GenericProduct",
        DisplayName = "Generic Product",
        Description = "Generic product supports mutiple products")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-23.png")]
    public class GenericProduct : ProductContent, IProductRecommendations, IFoundationContent
    {
        #region Content

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Sizing", GroupName = SystemTabNames.Content, Order = 5)]
        public virtual XhtmlString Sizing { get; set; }

        [CultureSpecific]
        [Display(Name = "Product teaser", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual XhtmlString ProductTeaser { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Brand", GroupName = SystemTabNames.Content, Order = 15)]
        public virtual string Brand { get; set; }

        [CultureSpecific]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Department", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Department { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", GroupName = SystemTabNames.Content, Order = 25)]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Legal disclaimer", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string LegalDisclaimer { get; set; }

        [CultureSpecific]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Product group", GroupName = SystemTabNames.Content, Order = 35)]
        public virtual string ProductGroup { get; set; }

        [CultureSpecific]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Product type name", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual string ProductTypeName { get; set; }

        [CultureSpecific]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Product type sub category", GroupName = SystemTabNames.Content, Order = 45)]
        public virtual string ProductTypeSubcategory { get; set; }

        [Display(Name = "On sale",
            GroupName = SystemTabNames.Content,
            Description = "Is on sale?",
            Order = 50)]
        public virtual bool OnSale { get; set; }

        [Display(Name = "New arrival",
            GroupName = SystemTabNames.Content,
            Description = "Is on a new arrival?",
            Order = 55)]
        public virtual bool NewArrival { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long description", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual XhtmlString LongDescription { get; set; }

        [CultureSpecific]
        [Display(Name = "Content area",
            GroupName = SystemTabNames.Content,
            Description = "This will display the content area.",
            Order = 65)]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Associations title",
            GroupName = SystemTabNames.Content,
            Description = "This is title of the Associations tab.",
            Order = 70)]
        public virtual string AssociationsTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Show recommendations",
            GroupName = SystemTabNames.Content,
            Description = "This will determine whether or not to show recommendations.",
            Order = 75)]
        public virtual bool ShowRecommendations { get; set; }

        [CultureSpecific]
        [Display(Name = "Product Status",
            GroupName = SystemTabNames.Content,
            Order = 80)]
        [SelectOne(SelectionFactoryType = typeof(ProductStatusSelectionFactory))]
        public virtual string ProductStatus { get; set; }

        #endregion

        #region Manufacturer

        [BackingType(typeof(PropertyString))]
        [Display(Name = "Manufacturer", GroupName = CommerceTabNames.Manufacturer, Order = 5)]
        public virtual string Manufacturer { get; set; }

        [CultureSpecific]
        [Display(Name = "Manufacturer parts warranty description", GroupName = CommerceTabNames.Manufacturer, Order = 10)]
        public virtual string ManufacturerPartsWarrantyDescription { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "Model", GroupName = CommerceTabNames.Manufacturer, Order = 15)]
        public virtual string Model { get; set; }

        [Display(Name = "Model year", GroupName = CommerceTabNames.Manufacturer, Order = 20)]
        [BackingType(typeof(PropertyString))]
        public virtual string ModelYear { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "Warranty", GroupName = CommerceTabNames.Manufacturer, Order = 25)]
        public virtual string Warranty { get; set; }

        #endregion

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
            ProductStatus = "Active";
        }
    }
}