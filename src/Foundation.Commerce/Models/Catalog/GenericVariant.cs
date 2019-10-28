using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Commerce.Models.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(DisplayName = "Generic Variant", GUID = "1aaa2c58-c424-4c37-81b0-77e76d254eb0", Description = "Generic variant supports multiple variation types")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-23.png")]
    public class GenericVariant : VariationContent, IProductRecommendations
    {

        [Searchable]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Size", Order = 5)]
        public virtual string Size { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Color", Order = 10)]
        public virtual string Color { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 15)]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Content area", Description = "This will display the content area.", Order = 20)]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Associations title", Description = "This is title of the Associations tab.", Order = 25)]
        public virtual string AssociationsTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Show recommendations", Description = "This will determine whether or not to show recommendations.", Order = 30)]
        public virtual bool ShowRecommendations { get; set; }

        [Required]
        [Display(Name = "Virtual product mode", Order = 35)]
        [SelectOne(SelectionFactoryType = typeof(VirtualVariantTypeSelectionFactory))]
        public virtual string VirtualProductMode { get; set; }

        [Display(Name = "Virtual product role", Order = 40)]
        [SelectOne(SelectionFactoryType = typeof(ElevatedRoleSelectionFactory))]
        [BackingType(typeof(PropertyString))]
        public virtual string VirtualProductRole { get; set; }

        #region Manufacturer

        [Display(Name = "Mpn", GroupName = CommerceTabNames.Manufacturer, Order = 5)]
        [BackingType(typeof(PropertyString))]
        public virtual string Mpn { get; set; }

        [Display(Name = "Package quantity", GroupName = CommerceTabNames.Manufacturer, Order = 10)]
        [BackingType(typeof(PropertyString))]
        public virtual string PackageQuantity { get; set; }

        [Display(Name = "Part number", GroupName = CommerceTabNames.Manufacturer, Order = 15)]
        [BackingType(typeof(PropertyString))]
        public virtual string PartNumber { get; set; }

        [Display(Name = "Region code", GroupName = CommerceTabNames.Manufacturer, Order = 20)]
        [BackingType(typeof(PropertyString))]
        public virtual string RegionCode { get; set; }

        [Display(Name = "Sku", GroupName = CommerceTabNames.Manufacturer, Order = 25)]
        [BackingType(typeof(PropertyString))]
        public virtual string Sku { get; set; }

        [Display(Name = "Subscription length", GroupName = CommerceTabNames.Manufacturer, Order = 30)]
        [BackingType(typeof(PropertyString))]
        public virtual string SubscriptionLength { get; set; }

        [Display(Name = "Upc", GroupName = CommerceTabNames.Manufacturer, Order = 35)]
        [BackingType(typeof(PropertyString))]
        public virtual string Upc { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            VirtualProductMode = "None";
            VirtualProductRole = "None";
            AssociationsTitle = "You May Also Like";
        }

    }
}