using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.Blocks;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Blocks
{
    [ContentType(
            DisplayName = "Configurable Product List",
            GUID = "8BD1CF05-4980-4BA2-9304-C0EAF946DAD5",
            Description = "Configurable search block for all products, allows generic filtering.",
            GroupName = "Commerce")]
    [ImageUrl("~/assets/icons/cms/pages/search.png")]
    public class ProductSearchBlock : FoundationBlockData
    {
        private int _startingIndex = 0;

        [Display(Order = 10, Name = "Heading")]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(Order = 20, Name = "Search Term")]
        [CultureSpecific]
        public virtual string SearchTerm { get; set; }

        [Display(Name = "Number of Results",
            Description = "The number of products to show in the list. Default is 6.",
            Order = 30)]
        [CultureSpecific]
        public virtual int ResultsPerPage { get; set; }

        [Display(Name = "Items per Row",
            Description = "The number of products to show in a row. Default is 3.",
            Order = 40)]
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(ProductSearchBlockItemsPerRowSelectionFactory))]
        public virtual int ItemsPerRow { get; set; }

        [Display(Name = "Categories",
            Description = "Root categories to get products from, includes sub categories",
            GroupName = SystemTabNames.Content, Order = 50)]
        [AllowedTypes(typeof(NodeContent))]
        public virtual ContentArea Nodes { get; set; }

        [Display(Name = "Filters",
            Description = "Filters to apply to the search result",
            Order = 60)]
        public virtual ContentArea Filters { get; set; }

        [Display(Name = "Priority Products",
            Description = "Products to put first in the list.",
            Order = 70)]
        [AllowedTypes(typeof(EntryContentBase))]
        public virtual ContentArea PriorityProducts { get; set; }

        [Display(Order = 80,
            Name = "Min Price",
            Description = "The minimum price in the current market currency")]
        [CultureSpecific]
        public virtual int MinPrice { get; set; }

        [Display(Order = 90,
            Name = "Max Price",
            Description = "The maximum price in the current market currency")]
        [CultureSpecific]
        public virtual int MaxPrice { get; set; }

        public void SetIndex(int index) => _startingIndex = index;

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ResultsPerPage = 6;
            ItemsPerRow = 3;
        }
    }
}
