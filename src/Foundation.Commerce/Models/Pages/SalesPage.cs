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
    [ContentType(DisplayName = "Sales Page",
        GUID = "9f6352bc-eea4-416a-bf76-144037c7d3db",
        Description = "Show all items on sale",
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-21.png")]
    public class SalesPage : FoundationPageData
    {
        [Display(Name = "Allow paging", Order = 210)]
        public virtual bool AllowPaging { get; set; }

        [PositiveNumber]
        [Display(Name = "Page size", Order = 220)]
        public virtual int PageSize { get; set; }

        //
        // Summary:
        // Gets or sets the list of included catalog items (catalogs, categories or entries).
        [AllowedTypes(typeof(NodeContent), typeof(ProductContent), typeof(PackageContent), typeof(CatalogContent))]
        [Display(Name = "Manual inclusion", Order = 230)]
        [DistinctList]
        public virtual IList<ContentReference> ManualInclusion { get; set; }

        //
        // Summary:
        //The manual inclusion products based on the Manual Inclusion Ordering.
        [Display(Name = "Manual inclusion ordering", Order = 240)]
        [SelectOne(SelectionFactoryType = typeof(InclusionOrderingSelectionFactory))]
        public virtual string ManualInclusionOrdering { get; set; }

        //
        // Summary:
        // Gets or sets the list of excluded catalog items (catalogs, categories or entries).
        [AllowedTypes(typeof(NodeContent), typeof(ProductContent), typeof(PackageContent), typeof(CatalogContent))]
        [Display(Name = "Manual exclusion", Order = 250)]
        [DistinctList]
        public virtual IList<ContentReference> ManualExclusion { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ManualInclusionOrdering = InclusionOrdering.Beginning;
            PageSize = 12;
        }
    }
}