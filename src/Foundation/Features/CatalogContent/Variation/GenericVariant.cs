using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Commerce.Models.EditorDescriptors;
using Foundation.Features.Blocks.ElevatedRoleBlock;
using Foundation.Features.Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.CatalogContent.Variation
{
    [CatalogContentType(DisplayName = "Generic Variant", GUID = "1aaa2c58-c424-4c37-81b0-77e76d254eb0", Description = "Generic variant supports multiple variation types")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-23.png")]
    public class GenericVariant : VariationContent, IProductRecommendations, IFoundationContent, IDashboardItem
    {
        [Tokenize]
        [Searchable]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Size", Order = 5)]
        public virtual string Size { get; set; }

        [Tokenize]
        [Searchable]
        [CultureSpecific]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Color", Order = 10)]
        public virtual string Color { get; set; }

        [Tokenize]
        [Searchable]
        [CultureSpecific]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 15)]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Content area", Order = 20)]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Associations title", Order = 25)]
        public virtual string AssociationsTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Show recommendations", Order = 30)]
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

        [Display(Name = "Mpn", GroupName = Infrastructure.TabNames.Manufacturer, Order = 5)]
        [BackingType(typeof(PropertyString))]
        public virtual string Mpn { get; set; }

        [Display(Name = "Package quantity", GroupName = Infrastructure.TabNames.Manufacturer, Order = 10)]
        [BackingType(typeof(PropertyString))]
        public virtual string PackageQuantity { get; set; }

        [Display(Name = "Part number", GroupName = Infrastructure.TabNames.Manufacturer, Order = 15)]
        [BackingType(typeof(PropertyString))]
        public virtual string PartNumber { get; set; }

        [Display(Name = "Region code", GroupName = Infrastructure.TabNames.Manufacturer, Order = 20)]
        [BackingType(typeof(PropertyString))]
        public virtual string RegionCode { get; set; }

        [Display(Name = "Sku", GroupName = Infrastructure.TabNames.Manufacturer, Order = 25)]
        [BackingType(typeof(PropertyString))]
        public virtual string Sku { get; set; }

        [Display(Name = "Subscription length", GroupName = Infrastructure.TabNames.Manufacturer, Order = 30)]
        [BackingType(typeof(PropertyString))]
        public virtual string SubscriptionLength { get; set; }

        [Display(Name = "Upc", GroupName = Infrastructure.TabNames.Manufacturer, Order = 35)]
        [BackingType(typeof(PropertyString))]
        public virtual string Upc { get; set; }

        #endregion

        #region Implement IFoundationContent

        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = Infrastructure.TabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = Infrastructure.TabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        [Display(Name = "CSS files", GroupName = Infrastructure.TabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Display(Name = "CSS", GroupName = Infrastructure.TabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        [Display(Name = "Script files", GroupName = Infrastructure.TabNames.Scripts, Order = 100)]
        public virtual LinkItemCollection ScriptFiles { get; set; }

        [UIHint(UIHint.Textarea)]
        [Display(GroupName = Infrastructure.TabNames.Scripts, Order = 200)]
        public virtual string Scripts { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            VirtualProductMode = "None";
            VirtualProductRole = "None";
            AssociationsTitle = "You May Also Like";
        }

        public void SetItem(ItemModel itemModel)
        {
            itemModel.Description = Description?.ToHtmlString();
            itemModel.Image = CommerceMediaCollection.FirstOrDefault()?.AssetLink;
        }
    }
}