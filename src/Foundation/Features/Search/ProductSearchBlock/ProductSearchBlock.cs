using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Commerce.Models.EditorDescriptors;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Search.ProductSearchBlock
{
    [ContentType(DisplayName = "Product Search Block",
        GUID = "8BD1CF05-4980-4BA2-9304-C0EAF946DAD5",
        Description = "Configurable search block for all products, allows generic filtering",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/search.png")]
    public class ProductSearchBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(Order = 10)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [Display(Name = "Search term", Order = 20)]
        public virtual string SearchTerm { get; set; }

        [CultureSpecific]
        [Display(Name = "Number of results", Description = "The number of products to show in the list, default is 6", Order = 30)]
        public virtual int ResultsPerPage { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(ProductSearchBlockItemsPerRowSelectionFactory))]
        [Display(Name = "Results per row", Description = "The number of products to show in a row, default is 3", Order = 40)]
        public virtual int ItemsPerRow { get; set; }

        [AllowedTypes(typeof(NodeContent))]
        [Display(Name = "Catalog categories", Description = "Root categories to get products from, includes sub categories", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual ContentArea Nodes { get; set; }

        [Display(Name = "Sort order", Description = "Sort order to apply to the search result", Order = 55)]
        [SelectOne(SelectionFactoryType = typeof(SortOrderSelectionFactory))]
        public virtual string SortOrder { get; set; }

        [Display(Description = "Filters to apply to the search result", Order = 60)]
        public virtual ContentArea Filters { get; set; }

        [AllowedTypes(typeof(EntryContentBase))]
        [Display(Name = "Priority products", Description = "Products to put first in the list", Order = 70)]
        public virtual ContentArea PriorityProducts { get; set; }

        [Display(Name = "Discontinued products mode", Description = "Handle discontinued products to show in the list", Order = 75)]
        [SelectOne(SelectionFactoryType = typeof(DiscontinuedProductModeSelectionFactory))]
        public virtual string DiscontinuedProductsMode { get; set; }

        [CultureSpecific]
        [Display(Name = "Minimum price", Description = "The minimum price in the current market currency", Order = 80)]
        public virtual int MinPrice { get; set; }

        [CultureSpecific]
        [Display(Name = "Maximum price", Description = "The maximum price in the current market currency", Order = 90)]
        public virtual int MaxPrice { get; set; }

        [SelectMany(SelectionFactoryType = typeof(BrandSelectionFactory))]
        [Display(Name = "Brand filter", Description = "Filter based on all available brands", Order = 100)]
        public virtual string BrandFilter { get; set; }

        private int _startingIndex = 0;
        public void SetIndex(int index) => _startingIndex = index;

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ResultsPerPage = 6;
            ItemsPerRow = 3;
            SortOrder = "None";
            DiscontinuedProductsMode = "Hide";
        }
    }
}
