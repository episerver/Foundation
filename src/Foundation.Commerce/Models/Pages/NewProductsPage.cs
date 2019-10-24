using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.Marketing.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.Pages;
using Foundation.Commerce.Models.EditorDescriptors;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Foundation.Commerce.Models.EditorDescriptors.InclusionOrderingSelectionFactory;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "New Products Page",
        GUID = "3ce903a3-3d48-4fe3-92f5-14b5e6f393b5",
        Description = "Show the top new products by sorted by the creation date",
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-21.png")]
    public class NewProductsPage : FoundationPageData
    {
        [PositiveNumber]
        [Display(Name = "Number of products", Order = 210)]
        public virtual int NumberOfProducts { get; set; }

        [Display(Name = "Allow paging", Order = 220)]
        public virtual bool AllowPaging { get; set; }

        [PositiveNumber]
        [Display(Name = "Page size", Order = 230)]
        public virtual int PageSize { get; set; }

        //
        // Summary:
        // Gets or sets the list of included catalog items (catalogs, categories or entries).
        [AllowedTypes(typeof(CatalogContent), typeof(NodeContent), typeof(ProductContent), typeof(PackageContent))]
        [Display(Name = "Manual inclusion", Order = 240)]
        [DistinctList]
        public virtual IList<ContentReference> ManualInclusion { get; set; }

        //
        // Summary:
        //The manual inclusion products based on the Manual Inclusion Ordering.
        [Display(Name = "Manual inclusion ordering", Order = 250)]
        [SelectOne(SelectionFactoryType = typeof(InclusionOrderingSelectionFactory))]
        public virtual string ManualInclusionOrdering { get; set; }

        //
        // Summary:
        // Gets or sets the list of excluded catalog items (catalogs, categories or entries).
        [AllowedTypes(typeof(CatalogContent), typeof(NodeContent), typeof(ProductContent), typeof(PackageContent))]
        [Display(Name = "Manual exclusion", Order = 260)]
        [DistinctList]
        public virtual IList<ContentReference> ManualExclusion { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ManualInclusionOrdering = InclusionOrdering.Beginning;
            NumberOfProducts = 12;
            PageSize = 12;
        }
    }
}