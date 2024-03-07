﻿using Foundation.Features.CatalogContent;

namespace Foundation.Features.Search.ProductSearchBlock
{
    public class ProductSearchResultViewModel : BlockViewModel<ProductSearchBlock>
    {
        public ProductSearchResultViewModel(ProductSearchBlock currentBlock) : base(currentBlock)
        {
        }

        public string Heading { get; set; }
        public int ItemsPerRow { get; set; }
        public List<ProductTileViewModel> Products { get; set; }
    }
}
