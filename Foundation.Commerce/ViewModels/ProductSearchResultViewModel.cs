using Foundation.Cms.ViewModels;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Blocks;
using System.Collections.Generic;

namespace Foundation.Commerce.ViewModels
{
    public class ProductSearchResultViewModel : BlockViewModel<ProductSearchBlock>
    {
        public ProductSearchResultViewModel(ProductSearchBlock currentBlock) : base(currentBlock)
        {
            PaddingStyles = currentBlock.PaddingStyles;
        }

        public string Heading { get; set; }
        public int ItemsPerRow { get; set; }
        public string PaddingStyles { get; set; }
        public List<ProductTileViewModel> Products { get; set; }
    }
}
