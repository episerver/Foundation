using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(
        GUID = "e638670d-3f73-4867-8745-1125dcc066ca",
        MetaClassName = "GenericProduct",
        DisplayName = "Generic Product",
        Description = "Generic product supports mutiple products")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-23.png")]
    public class GenericProduct : ProductContent, IProductRecommendations
    {
        [Display(GroupName = SystemTabNames.Content, Order = 7)]
        [BackingType(typeof(PropertyString))]
        [CultureSpecific]
        public virtual string Department { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 24)]
        [CultureSpecific]
        public virtual string LegalDisclaimer { get; set; }

        [Display(GroupName = CommerceTabNames.Manufacturer, Order = 25)]
        [BackingType(typeof(PropertyString))]
        public virtual string Manufacturer { get; set; }

        [Display(GroupName = CommerceTabNames.Manufacturer, Order = 26)]
        [CultureSpecific]
        public virtual string ManufacturerPartsWarrantyDescription { get; set; }

        [Display(GroupName = CommerceTabNames.Manufacturer, Order = 27)]
        [BackingType(typeof(PropertyString))]
        public virtual string Model { get; set; }

        [Display(GroupName = CommerceTabNames.Manufacturer, Order = 28)]
        [BackingType(typeof(PropertyString))]
        public virtual string ModelYear { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 31)]
        [BackingType(typeof(PropertyString))]
        [CultureSpecific]
        public virtual string ProductGroup { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 32)]
        [BackingType(typeof(PropertyString))]
        [CultureSpecific]
        public virtual string ProductTypeName { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 33)]
        [BackingType(typeof(PropertyString))]
        [CultureSpecific]
        public virtual string ProductTypeSubcategory { get; set; }

        [Display(Order = 40, GroupName = CommerceTabNames.Manufacturer)]
        [BackingType(typeof(PropertyString))]
        public virtual string Warranty { get; set; }

        [Display(Name = "On Sale", Order = 41, Description = "Is on sale?")]
        public virtual bool OnSale { get; set; }

        [Display(Name = "New Arrival", Order = 42, Description = "Is on a new arroval?")]
        public virtual bool NewArrival { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Sizing", Order = 4, GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Sizing { get; set; }

        [CultureSpecific]
        [Display(Name = "Product Teaser", Order = 5)]
        public virtual XhtmlString ProductTeaser { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Brand", Order = 5)]
        public virtual string Brand { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 9)]
        public virtual XhtmlString Description { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long Description", Order = 43)]
        public virtual XhtmlString LongDescription { get; set; }

        [CultureSpecific]
        [Display(Name = "Content Area", Order = 44, Description = "This will display the content area.")]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Show Recommendations", Order = 50, Description = "This will determine whether or not to show recommendations.")]
        public virtual bool ShowRecommendations { get; set; }




    }
}