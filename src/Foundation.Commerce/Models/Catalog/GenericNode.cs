using EPiBootstrapArea;
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
    [CatalogContentType(DisplayName = "Generic Node", GUID = "4ac27ad4-bf60-4ea0-9a77-28a89d38d3fd", Description = "")]
    public class GenericNode : NodeContent, IProductRecommendations, IFoundationContent
    {
        [CultureSpecific]
        [Display(Name = "Long name", GroupName = SystemTabNames.Content, Order = 5)]
        [BackingType(typeof(PropertyString))]
        public virtual string LongName { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Teaser { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", GroupName = SystemTabNames.Content, Order = 15)]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(
           Name = "Featured products",
           GroupName = SystemTabNames.Content,
           Order = 4)]
        [AllowedTypes(AllowedTypes = new [] {typeof(ProductContent), typeof(NodeContent), typeof(PackageContent), typeof(BundleContent)})]
        public virtual ContentArea FeaturedProducts { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Top content area",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual ContentArea TopContentArea { get; set; }

        [Display(Name = "Partial page size", Order = 25)]
        public virtual int PartialPageSize { get; set; }

        [CultureSpecific]
        [Display(Name = "Show recommendations", Order = 30)]
        public virtual bool ShowRecommendations { get; set; }

        [Display(Name = "Default template", Order = 40)]
        [SelectOne(SelectionFactoryType = typeof(GenericNodeSelectionFactory))]
        public virtual string DefaultTemplate { get; set; }

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
            DefaultTemplate = "List";
        }
    }
}